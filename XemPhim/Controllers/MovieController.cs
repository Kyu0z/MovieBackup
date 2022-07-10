using System.Linq;
using System.Web.Http;
using XemPhim.Models;

namespace XemPhim.Controllers
{
    [RoutePrefix("movies")]
    [Authorize]
    public class MovieController : CRUDController<Movie>
    {
        public MovieController()
        {
        }

        protected override IOrderedQueryable<Movie> getDbSet()
        {
            return this.dbContext.Movies.OrderBy(x => x.CreatedAt);
        }

        [Route("test")]
        [HttpGet]
        public string Test()
        {
            return "Ok";
        }
    }
}
