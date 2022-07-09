using System.Linq;
using System.Web.Http;
using XemPhim.Interfaces.CRUD;

namespace XemPhim.Controllers
{
    [Authorize]
    public abstract class CRUDController<T> : BaseApiController where T : class
    {
        public CRUDController()
        {
        }

        protected abstract IOrderedQueryable<T> getDbSet();

        [Route("")]
        [HttpGet]
        public ListOf<T> GetAll(int size = 10, int page = 1)
        {
            return ListOf<T>.From(this.getDbSet(), Paging.From(size, page));
        }
    }
}
