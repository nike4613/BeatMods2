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
using System.Net;
using Newtonsoft.Json.Linq;
using Octokit;
using System.Collections.ObjectModel;

namespace BeatMods2.Controllers
{
    [Route("api/users")]
    [ApiController, Authorize]
    public class UsersController : ControllerBase
    {
        private GitHubAuth githubAuthSettings;
        private CoreAuth coreAuthSettings;
        private SymmetricAlgorithm stateEncAlgo;
        private ModRepoContext repoContext;
        private GitHubClient client;

        public UsersController(GitHubAuth ghAuth,
            CoreAuth coreAuth,
            GitHubClient client,
            SymmetricAlgorithm encAlgo, 
            ModRepoContext context)
        {
            githubAuthSettings = ghAuth;
            coreAuthSettings = coreAuth;
            this.client = client;
            stateEncAlgo = encAlgo;
            repoContext = context;
            UpdateCurrentRandomData();
        }

        private string CurrentRandomData = "hello there, you shouldn't see this!~";
        private void UpdateCurrentRandomData()
        {
            CurrentRandomData = Utils.GetCryptoRandomHexString(16);
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
            public void OnError(StreamingContext context, ErrorContext error)
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
                try
                {
                    using var mstream = new MemoryStream(Utils.StringToByteArray(input));
                    using var cstream = new CryptoStream(mstream, dec, CryptoStreamMode.Read);
                    using var treader = new StreamReader(cstream);
                    using var jreader = new JsonTextReader(treader);
                    return new JsonSerializer().Deserialize<StateData>(jreader);
                }
                catch (Exception)
                {
                    return new StateData { IsValid = false };
                }
            }
        }

        public const string LoginName = "Api_UserLogin";
        [HttpGet("login", Name = LoginName), AllowAnonymous]
        public IActionResult Login([FromQuery] string success, 
            [FromQuery] string? failure = null, 
            [FromQuery] string? userData = null)
        {
            var req = new OauthLoginRequest(githubAuthSettings.ClientId) 
            {
                State = new StateData 
                    {
                        RandomState = CurrentRandomData,
                        SuccessCallback = success,
                        FailureCallback = failure,
                        UserData = userData
                    }.Encrypt(stateEncAlgo),
                AllowSignup = false,
                RedirectUri = new Uri(Url.AbsoluteRouteUrl(LoginCallbackName))
            };
            foreach (var scope in githubAuthSettings.OauthScopes)
                req.Scopes.Add(scope);
            
            return Ok(new { AuthTarget = client.Oauth.GetGitHubLoginUrl(req) });
        }

        public const string LoginCallbackName = "Api_UserLoginCallback";
        [HttpGet("login_callback", Name = LoginCallbackName), AllowAnonymous]
        public async Task<IActionResult> LoginComplete([FromQuery] string code, [FromQuery] string state)
        {
            const string UserDataParam = "data";
            const string SuccessParam = "successful";
            const string ErrorParam = "error";
            const string CodeParam = "code";

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
                failureCb = QueryHelpers.AddQueryString(successCb, SuccessParam, "false");
                successCb = QueryHelpers.AddQueryString(successCb, SuccessParam, "true");
            }

            OauthToken token;
            try
            {
                token = await client.Oauth.CreateAccessToken(
                    new OauthTokenRequest(githubAuthSettings.ClientId,
                        githubAuthSettings.ClientSecret, code)
                    {
                        RedirectUri = new Uri(Url.AbsoluteRouteUrl(LoginCallbackName))
                    });
            } // will not catch NotFound, Validation, or LegalRestriction because those are hard errors
            catch (AuthorizationException e)
            {
                return Redirect(QueryHelpers.AddQueryString(failureCb, ErrorParam,
                    $"GitHub returned Unauthorized: {e.Message}"));
            }
            catch (ForbiddenException e)
            {
                return Redirect(QueryHelpers.AddQueryString(failureCb, ErrorParam,
                    $"GitHub returned Forbidden: {e.Message}"));
            }
            catch (ApiException e)  
            {
                return Redirect(QueryHelpers.AddQueryString(failureCb, ErrorParam,
                    $"GitHub returned error: {e.Message}"));
            }

            if (token.TokenType != "bearer") // don't know what to do with it
                return Redirect(QueryHelpers.AddQueryString(failureCb, ErrorParam,
                    $"API returned unknown token type {token.TokenType}"));

            var newCode = Utils.GetCryptoRandomHexString(8); // keep it somewhat short
            while (repoContext.AuthCodes.Any(s => s.Code == newCode)) // in the odd case that 2 exist at once
                newCode = Utils.GetCryptoRandomHexString(8);

            repoContext.AuthCodes.Add(new AuthCodeTempStore
            {
                Code = newCode,
                GitHubBearer = token.AccessToken
            });
            
            await repoContext.SaveChangesAsync(); // these saves feel kinda gross ngl
            // TODO: will this ever cause a race condition?

            return Redirect(QueryHelpers.AddQueryString(successCb, CodeParam, newCode));
        }

        public class AuthenticateRequest
        {
            [JsonProperty("code")]
            public string Code = "";
        }

        public const string AuthenticateName = "Api_UserAuthenticate";
        [HttpPost("authenticate", Name = AuthenticateName), AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest req)
        {
            repoContext.AuthCodes.ClearExpired(); // removed expired codes

            var code = req.Code;

            var auth = repoContext.AuthCodes.FirstOrDefault(s => s.Code == code);
            if (auth == null)
                return NotFound(new {
                    error = "Code not found"
                });
            
            repoContext.AuthCodes.Remove(auth);

            var ghKey = auth.GitHubBearer;

            // TODO: do something with the key (get/create user and generate JWT)

            var user = repoContext.Users.FirstOrDefault(u => u.GithubToken == ghKey);

            await repoContext.SaveChangesAsync();
            return Ok();
        }
    }
}