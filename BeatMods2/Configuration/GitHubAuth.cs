using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeatMods2.Configuration
{
    public class GitHubAuth
    {
        public Uri BaseUri { get; set; }
        public string OauthAuthorize { get; set; }
        public string OauthAccess { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string[] OauthScopes { get; set; }
    }
}
