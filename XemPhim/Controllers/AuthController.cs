using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Web;
using XemPhim.Interfaces.Auth;
using XemPhim.Models;

namespace XemPhim.Controllers
{
    public class AuthController : BaseApiController
    {
        public AuthController()
        {
        }

        protected ApplicationSignInManager getSignInManager()
        {
            return HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
        }

        public LoginResult PostLogin(LoginData data)
        {
            SignInStatus status = this.getSignInManager().PasswordSignIn(data.Username, data.Password, isPersistent: false, shouldLockout: false);
            LoginResult result = new LoginResult();
            switch (status)
            {
                case SignInStatus.Success:
                    String username = this.getSignInManager().AuthenticationManager.User.Identity.Name;
                    result.Success = true;
                    result.Message = "Đăng nhập thành công";
                    ApplicationUser user = this.dbContext.Users.Where(x => x.UserName == username).First();
                    result.Data = new LoginResultData()
                    {
                        User = UserData.From(user),
                        Token = TokenData.From(new AuthTokenManager(this.dbContext).CreateForUser(user)),
                    };
                    break;
                case SignInStatus.LockedOut:
                case SignInStatus.RequiresVerification:
                case SignInStatus.Failure:
                    result.Message = "Đăng nhập thất bại";
                    break;
            }
            if (result.Message == null)
            {
                result.Message = "Đăng nhập lỗi không xác định";
            }
            return result;
        }
    }
}
