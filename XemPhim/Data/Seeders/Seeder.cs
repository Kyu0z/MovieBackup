using System.Threading.Tasks;
using XemPhim.Models;

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
            Task<DBContext> task = GetDbContextAsync();
            task.Wait();
            return task.Result;
        }

        public override async Task Seed()
        {
            await new RoleSeeder(this.dbContext).Seed();
            await new UserSeeder(this.dbContext).Seed();
        }
    }
}