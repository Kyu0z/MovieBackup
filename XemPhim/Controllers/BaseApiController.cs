using System.Web.Http;
using XemPhim.Data.Seeders;
using XemPhim.Models;

namespace XemPhim.Controllers
{
    public class BaseApiController : ApiController
    {
        protected readonly DBContext dbContext;

        public BaseApiController()
        {
            this.dbContext = Seeder.GetDbContext();
        }
    }
}
