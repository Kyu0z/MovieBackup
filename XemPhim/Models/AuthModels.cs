using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XemPhim.Models
{
    public class AuthTokenManager
    {
        protected readonly DBContext dbContext;

        public AuthTokenManager(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public AuthToken CreateForUser(ApplicationUser user)
        {
            AuthToken token = new AuthToken()
            {
                AccessToken = user.UserName,
                User = user,
            };

            this.dbContext.AuthTokens.Add(token);
            this.dbContext.SaveChanges();

            return token;
        }
    }

    [Table("AuthTokens")]
    public class AuthToken
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

        [Required]
        public string AccessToken { get; set; }
    }
}