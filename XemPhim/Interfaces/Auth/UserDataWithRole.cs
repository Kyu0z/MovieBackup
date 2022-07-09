using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using XemPhim.Models;
using XemPhim.Utils;

namespace XemPhim.Interfaces.Auth
{
    public class UserDataWithRole : UserData
    {
        public string[] Roles { get; set; }

        public static UserDataWithRole From(ApplicationUser user, RoleStore<IdentityRole> roleStore)
        {
            UserData data = UserData.From(user);

            return new UserDataWithRole()
            {
                Id = data.Id,
                Username = data.Username,
                Email = data.Email,
                Roles = user.Roles.Select(x => AsyncHelper.RunSync(() => roleStore.FindByIdAsync(x.RoleId)).Name).ToArray(),
            };
        }
    }
}