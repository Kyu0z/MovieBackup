namespace XemPhim.Interfaces.Auth
{
    public class MeResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public UserData Data { get; set; }
    }
}