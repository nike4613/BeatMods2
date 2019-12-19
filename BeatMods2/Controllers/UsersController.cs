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

namespace BeatMods2.Controllers
{
    [Route("api/users")]
    [ApiController, Authorize]
    public class UsersController : ControllerBase
    {
        private GitHubAuth authSettings;
        private IHttpClientFactory httpFactory;

        public UsersController(GitHubAuth auth, IHttpClientFactory httpFac)
        {
            authSettings = auth;
            httpFactory = httpFac;
        }

        public class LoginResult
        {
            public Uri AuthTarget;
        }

        public const string LoginName = "Api_UserLogin";
        [HttpGet("login", Name = LoginName), AllowAnonymous]
        public ActionResult<LoginResult> Login()
        {
            var uri = new Uri(authSettings.BaseUri, authSettings.OauthAuthorize).ToString();
            uri = QueryHelpers.AddQueryString(uri, new Dictionary<string, string>
            {
                { "client_id", authSettings.ClientId },
                { "allow_signup", "false" },
                { "scope", string.Join(" ", authSettings.OauthScopes) },
                { "state", "TODO: generate random state" }, // TODO: generate random state and use it
                { "redirect_uri", Url.AbsoluteRouteUrl(LoginCallbackName) }
            });

            return new LoginResult { AuthTarget = new Uri(uri) };
        }

        private class GitHubAccessRequest
        {
            [JsonProperty("client_id")]
            public string ClientId;
            [JsonProperty("client_secret")]
            public string ClientSecret;
            [JsonProperty("code")]
            public string Code;
            [JsonProperty("state")]
            public string State;
        }
        private class GitHubAccesResponse
        {
            [JsonProperty("access_token")]
            public string Token;
            [JsonProperty("scope")]
            public string Scopes;
            [JsonProperty("token_type")]
            public string TokenType;
        }

        public const string LoginCallbackName = "Api_UserLoginCallback";
        [HttpGet("login_callback", Name = LoginCallbackName), AllowAnonymous]
        public async Task<IActionResult> LoginComplete([FromQuery] string code, [FromQuery] string state)
        {
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

            // TODO
            return Redirect(Url.RouteUrl("Page_LoginComplete"));
        }
    }
}