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
using System.Buffers;
using BeatMods2.Models;
using System.Runtime.Serialization;
using Newtonsoft.Json.Serialization;
using BeatMods2.Results;

namespace BeatMods2.Controllers
{
    [Route("api/users")]
    [ApiController, Authorize]
    public class UsersController : ControllerBase
    {
        private GitHubAuth authSettings;
        private IHttpClientFactory httpFactory;
        private SymmetricAlgorithm stateEncAlgo;
        private ModRepoContext repoContext;

        public UsersController(GitHubAuth auth, 
            IHttpClientFactory httpFac, 
            SymmetricAlgorithm encAlgo, 
            ModRepoContext context)
        {
            authSettings = auth;
            httpFactory = httpFac;
            stateEncAlgo = encAlgo;
            repoContext = context;
            UpdateCurrentRandomData();
        }

        private string CurrentRandomData = "hello there, you shouldn't see this!~";
        private void UpdateCurrentRandomData()
        {
            using var rand = new RNGCryptoServiceProvider();
            using var mem = MemoryPool<byte>.Shared.Rent(16);
            rand.GetBytes(mem.Memory.Span);
            CurrentRandomData = Utils.BytesToString(mem.Memory.Span);
        }

        private class StateData
        {
            public string RandomState = "";// Generated in UpdateCurrentRandomData, set in Login
            public string SuccessCallback = "";
            public string? FailureCallback = null;
            public string? UserData = null;

            [JsonIgnore]
            public bool IsValid = true;

            [OnError]
            internal void OnError(StreamingContext context, ErrorContext error)
            {
                IsValid = false;
                error.Handled = true;
            }

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
        public IActionResult Login([FromQuery] string success, 
            [FromQuery] string? failure = null, 
            [FromQuery] string? userData = null)
        {
            var uri = new Uri(authSettings.BaseUri!, authSettings.OauthAuthorize).ToString();
            uri = QueryHelpers.AddQueryString(uri, new Dictionary<string, string>
            {
                { "client_id", authSettings.ClientId },
                { "allow_signup", "false" },
                { "scope", string.Join(" ", authSettings.OauthScopes) },
                { "state", new StateData 
                    {
                        RandomState = CurrentRandomData,
                        SuccessCallback = success,
                        FailureCallback = failure,
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
            const string UserDataParam = "data";
            const string SuccessParam = "successful";

            var stateDe = StateData.Decrypt(stateEncAlgo, state);

            if (!stateDe.IsValid) 
                // state invalid; refuse to brew coffee
                return this.ImATeapot();

            var successCb = stateDe.SuccessCallback;
            if (stateDe.UserData != null)
                successCb = QueryHelpers.AddQueryString(successCb, UserDataParam, stateDe.UserData);

            var failureCb = stateDe.FailureCallback;
            if (failureCb != null && stateDe.UserData != null)
                failureCb = QueryHelpers.AddQueryString(failureCb, UserDataParam, stateDe.UserData);
            else if (failureCb == null) 
            {
                successCb = QueryHelpers.AddQueryString(successCb, SuccessParam, "true");
                failureCb = QueryHelpers.AddQueryString(successCb, SuccessParam, "false");
            }

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

            return Redirect(successCb);
        }
    }
}