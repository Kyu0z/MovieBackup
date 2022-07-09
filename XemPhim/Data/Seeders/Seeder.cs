using System.Threading.Tasks;
using XemPhim.Models;
using XemPhim.Utils;

namespace XemPhim.Data.Seeders
{
    public class Seeder : BaseSeeder
    {
        public Seeder() : base()
        {
        }

        public static async Task<DBContext> GetDbContextAsync()
        {
            Seeder seeder = new Seeder();
            await seeder.Seed();

            return seeder.dbContext;
        }

        public static DBContext GetDbContext()
        {
            return AsyncHelper.RunSync(() => GetDbContextAsync());
        }

        public override async Task Seed()
        {
            await new RoleSeeder(this.dbContext).Seed();
            await new UserSeeder(this.dbContext).Seed();
        }
    }
}