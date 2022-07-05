using XemPhim.Models;

namespace XemPhim.Interfaces.Auth
{
    public class TokenData
    {
        public string AccessToken { get; set; }

        public static TokenData From(AuthToken token)
        {
            return new TokenData()
            {
                AccessToken = token.AccessToken,
            };
        }
    }
}