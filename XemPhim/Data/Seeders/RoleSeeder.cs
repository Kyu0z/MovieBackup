using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Threading.Tasks;
using XemPhim.Models;

namespace XemPhim.Data.Seeders
{
    public class RoleSeeder : UserSeeder
    {
        public RoleSeeder(DBContext dbContext) : base(dbContext)
        {
        }

        protected RoleManager<IdentityRole> getIdentityRoleManager()
        {
            RoleStore<IdentityRole> store = new RoleStore<IdentityRole>(this.dbContext);
            return new RoleManager<IdentityRole>(store);
        }

        public override async Task Seed()
        {
            RoleManager<IdentityRole> roleManager = getIdentityRoleManager();

            foreach (string roleName in new List<string>() { "Admin", "Staff" })
            {
                if (roleManager.FindByName(roleName) == null)
                {
                    await this.getIdentityRoleManager().CreateAsync(new IdentityRole()
                    {
                        Name = roleName,
                    });
                }
            }
        }
    }
}