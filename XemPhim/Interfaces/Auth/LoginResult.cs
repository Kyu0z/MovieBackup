namespace XemPhim.Interfaces.Auth
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public LoginResultData Data { get; set; }
    }

    public class LoginResultData
    {
        public UserData User { get; set; }
        public TokenData Token { get; set; }
    }
}