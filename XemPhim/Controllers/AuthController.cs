using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using XemPhim.Interfaces.Auth;
using XemPhim.Interfaces.OAuth;
using XemPhim.Models;

namespace XemPhim.Controllers
{
    [RoutePrefix("auth")]
    [Authorize]
    public class AuthController : BaseApiController
    {
        public AuthController()
        {
        }

        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<LoginResult> PostLogin([FromBody] LoginData data)
        {
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri(Request.RequestUri.GetLeftPart(UriPartial.Authority))
            };

            FormUrlEncodedContent content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", data.Username),
                new KeyValuePair<string, string>("password", data.Password)
            });

            HttpResponseMessage tokenResult = await client.PostAsync("/api/auth/token", content);
            string tokenResultBody = await tokenResult.Content.ReadAsStringAsync();

            LoginResult result = new LoginResult();
            switch (tokenResult.StatusCode)
            {
                case HttpStatusCode.OK:
                    OAuthToken oauthToken = JsonConvert.DeserializeObject<OAuthToken>(tokenResultBody);
                    result.Success = true;
                    result.Message = "Đăng nhập thành công";
                    ApplicationUser user = this.dbContext.Users.Where(x => x.UserName == data.Username).First();
                    RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(this.dbContext);
                    result.Data = new LoginResultData()
                    {
                        User = UserDataWithRole.From(user, roleStore),
                        Token = oauthToken,
                    };
                    break;
                case HttpStatusCode.BadRequest:
                    OAuthError oauthError = JsonConvert.DeserializeObject<OAuthError>(tokenResultBody);
                    result.Message = "Đăng nhập thất bại";
                    result.Error = oauthError;
                    break;
                default:
                    result.Message = "Lỗi hệ thống";
                    break;
            }
            return result;
        }

        [Route("register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<RegisterResult> PostRegister([FromBody] RegisterData data)
        {
            ApplicationUserManager userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser user = new ApplicationUser
            {
                UserName = data.Username,
                Email = data.Email,
            };
            IdentityResult registerResult = await userManager.CreateAsync(user, data.Password);

            RegisterResult result = new RegisterResult();
            if (registerResult.Succeeded)
            {
                result.Success = true;
                result.Message = "Đăng ký thành công";
            }
            else
            {
                foreach (String error in registerResult.Errors)
                {
                    if (error != null)
                    {
                        result.Message = error;
                    }
                }
            }
            return result;
        }

        [Route("me")]
        [HttpGet]
        [AllowAnonymous]
        public MeResult GetMe()
        {
            MeResult result = new MeResult();
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                result.Success = true;
                result.Message = "Thành công";
                String username = HttpContext.Current.User.Identity.Name;
                ApplicationUser user = this.dbContext.Users.Where(x => x.UserName == username).First();
                RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(this.dbContext);
                result.Data = UserDataWithRole.From(user, roleStore);
            } else
            {
                result.Message = "Bạn chưa đăng nhập hoặc token không hợp lệ";
            }
            return result;
        }
    }
}
