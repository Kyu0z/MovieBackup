using XemPhim.Models;

namespace XemPhim.Interfaces.Auth
{
    public class UserData
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public static UserData From(ApplicationUser user)
        {
            return new UserData()
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
            };
        }
    }
}