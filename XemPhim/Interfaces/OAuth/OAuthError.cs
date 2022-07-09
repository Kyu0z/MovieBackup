using Newtonsoft.Json;

namespace XemPhim.Interfaces.OAuth
{
    public class OAuthError
    {
        public string Error { get; set; }

        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
    }
}