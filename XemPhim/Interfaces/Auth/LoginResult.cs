using Newtonsoft.Json;
using XemPhim.Interfaces.OAuth;

namespace XemPhim.Interfaces.Auth
{
    public class LoginResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public LoginResultData Data { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public OAuthError Error { get; set; }
    }

    public class LoginResultData
    {
        public UserData User { get; set; }
        public OAuthToken Token { get; set; }
    }
}