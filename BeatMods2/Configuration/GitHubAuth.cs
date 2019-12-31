using BeatMods2.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeatMods2.Configuration
{
    public class GitHubAuth
    {
        public const string LoginClient = "GitHub_Login";
        public const string ApiClient = "GitHub_Api";

        public string ClientId { get; set; } = "";
        public string ClientSecret { get; set; } = "";
        public string[] OauthScopes { get; set; } = Array.Empty<string>();

        public int[] StateEncKey { get; set; } = Array.Empty<int>();
        public byte[] StateEncKeyBytes => Utils.BytesFromInts(StateEncKey);
    }
}
