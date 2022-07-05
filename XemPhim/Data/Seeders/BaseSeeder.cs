using System.Threading.Tasks;
using XemPhim.Models;

namespace XemPhim.Data.Seeders
{
    public abstract class BaseSeeder
    {
        protected readonly DBContext dbContext;

        public BaseSeeder()
        {
            this.dbContext = DBContext.GetSington();
        }

        public BaseSeeder(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public abstract Task Seed();
    }
}