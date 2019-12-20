using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BeatMods2.Configuration;
using Microsoft.AspNetCore.WebUtilities;
using BeatMods2.Utilities;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace BeatMods2.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class UsersController : ControllerBase
    {
        private GitHubAuth authSettings;
        private IHttpClientFactory httpFactory;
        private SymmetricAlgorithm stateEncAlgo;

        public UsersController(GitHubAuth auth, IHttpClientFactory httpFac, SymmetricAlgorithm encAlgo)
        {
            authSettings = auth;
            httpFactory = httpFac;
            stateEncAlgo = encAlgo;
        }

        private class StateData
        {
            public string RandomState = "TODO";// TODO: generate random state and use it
            public string? ReturnTo = null;
            public string? UserData = null;

            public string Encrypt(SymmetricAlgorithm algo)
            {
                var enc = algo.CreateEncryptor();
                using var mstream = new MemoryStream();
                using (var cstream = new CryptoStream(mstream, enc, CryptoStreamMode.Write))
                using (var twriter = new StreamWriter(cstream))
                {
                    new JsonSerializer().Serialize(twriter, this);
                }

                if (mstream.TryGetBuffer(out var buffer))
                    return Utils.BytesToString(buffer);
                else throw new Exception("Could not get buffer of memory stream");
            }

            public static StateData Decrypt(SymmetricAlgorithm algo, string input)
            {
                var dec = algo.CreateDecryptor();
                using var mstream = new MemoryStream(Utils.StringToByteArray(input));
                using var cstream = new CryptoStream(mstream, dec, CryptoStreamMode.Read);
                using var treader = new StreamReader(cstream);
                using var jreader = new JsonTextReader(treader);
                return new JsonSerializer().Deserialize<StateData>(jreader);
            }
        }

        public const string LoginName = "Api_UserLogin";
        [HttpGet("login", Name = LoginName), AllowAnonymous]
        public IActionResult Login([FromQuery] string returnTo, [FromQuery] string? userData = null)
        {
            var uri = new Uri(authSettings.BaseUri!, authSettings.OauthAuthorize).ToString();
            uri = QueryHelpers.AddQueryString(uri, new Dictionary<string, string>
            {
                { "client_id", authSettings.ClientId },
                { "allow_signup", "false" },
                { "scope", string.Join(" ", authSettings.OauthScopes) },
                { "state", new StateData {
                        ReturnTo = returnTo,
                        UserData = userData
                    }.Encrypt(stateEncAlgo) }, 
                { "redirect_uri", Url.AbsoluteRouteUrl(LoginCallbackName) }
            });

            return Ok(new { AuthTarget = new Uri(uri) });
        }

        private class GitHubAccessRequest
        {
            [JsonProperty("client_id")]
            public string ClientId = "";
            [JsonProperty("client_secret")]
            public string ClientSecret = "";
            [JsonProperty("code")]
            public string Code = "";
            [JsonProperty("state")]
            public string State = "";
        }
        private class GitHubAccesResponse
        {
            [JsonProperty("access_token")]
            public string Token = "";
            [JsonProperty("scope")]
            public string Scopes = "";
            [JsonProperty("token_type")]
            public string TokenType = "";
        }

        public const string LoginCallbackName = "Api_UserLoginCallback";
        [HttpGet("login_callback", Name = LoginCallbackName), AllowAnonymous]
        public async Task<IActionResult> LoginComplete([FromQuery] string code, [FromQuery] string state)
        {
            var stateDe = StateData.Decrypt(stateEncAlgo, state);

            var returnTarget = stateDe.ReturnTo ?? Url.RouteUrl("Page_LoginComplete");
            if (stateDe.UserData != null)
                returnTarget = QueryHelpers.AddQueryString(returnTarget, "data", stateDe.UserData);

            var client = httpFactory.CreateClient(GitHubAuth.LoginClient);
            var request = new HttpRequestMessage(HttpMethod.Post, authSettings.OauthAccess)
            {
                Content = new StringContent(JsonConvert.SerializeObject(
                    new GitHubAccessRequest
                    {
                        ClientId = authSettings.ClientId,
                        ClientSecret = authSettings.ClientSecret,
                        Code = code,
                        State = state
                    }), Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Accept", "application/json");

            var response = await client.SendAsync(request);

            // TODO: do checking here
            GitHubAccesResponse ghResponse;

            using (var treader = new StreamReader(await response.Content.ReadAsStreamAsync()))
            using (var reader = new JsonTextReader(treader))
                ghResponse = new JsonSerializer().Deserialize<GitHubAccesResponse>(reader);

            // TODO: use github response

            return Redirect(returnTarget);
        }
    }
}