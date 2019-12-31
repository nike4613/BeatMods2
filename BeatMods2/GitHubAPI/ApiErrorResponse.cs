using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BeatMods2.GitHubAPI
{
    public class BasicErrorResponse
    {
        [JsonProperty("error")]
        public string Error = "";
    }

    public class ApiErrorResponse
    {
        public enum ErrorType
        {
            incorrect_client_credentials, bad_verification_code
        }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("error")]
        public ErrorType Error;

        [JsonProperty("error_description")]
        public string ErrorDescription = "";

        [JsonProperty("error_uri")]
        public string ErrorUri = "";
    }
}