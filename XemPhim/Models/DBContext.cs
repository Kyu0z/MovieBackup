using System.Data.Entity;

namespace XemPhim.Models
{
    public partial class DBContext : ApplicationDbContext
    {
        protected static DBContext sington;

        public virtual DbSet<AuthToken> AuthTokens { get; set; }

        public static new DBContext Create()
        {
            return new DBContext();
        }

        public static DBContext GetSington()
        {
            if (sington == null)
            {
                sington = Create();
            }
            return sington;
        }
    }
}