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

namespace BeatMods2.Controllers
{
    [Route("api/users")]
    [ApiController, Authorize]
    public class UsersController : ControllerBase
    {
        private GitHubAuth authSettings;

        public UsersController(GitHubAuth auth)
        {
            authSettings = auth;
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
                { "state", "TODO: generate random state" }, // TODO: generate random state
                { "redirect_uri", Url.AbsoluteRouteUrl(LoginCallbackName) }
            });

            return new LoginResult { AuthTarget = new Uri(uri) };
        }

        public const string LoginCallbackName = "Api_UserLoginCallback";
        [HttpGet("login_callback", Name = LoginCallbackName), AllowAnonymous]
        public IActionResult LoginComplete()
        {
            // TODO
            return Redirect(Url.RouteUrl("Page_LoginComplete"));
        }
    }
}