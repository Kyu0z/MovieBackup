using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Threading.Tasks;
using XemPhim.Models;

namespace XemPhim.Data.Seeders
{
    public class UserSeeder : BaseSeeder
    {
        public UserSeeder(DBContext dbContext) : base(dbContext)
        {
        }

        protected ApplicationUserManager getApplicationUserManager()
        {
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(this.dbContext);
            return new ApplicationUserManager(store);
        }

        public override async Task Seed()
        {
            ApplicationUserManager userManager = this.getApplicationUserManager();

            if (await userManager.FindByNameAsync("admin") == null)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Email = "admin@admin.com",
                    UserName = "admin",
                    EmailConfirmed = true,
                };
                await userManager.CreateAsync(user, "admin123");

                foreach (IdentityRole role in this.dbContext.Roles.ToList())
                {
                    await userManager.AddToRoleAsync(user.Id, role.Name);
                }
            }
        }
    }
}