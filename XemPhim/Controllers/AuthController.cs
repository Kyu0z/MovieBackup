using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using XemPhim.Interfaces.Auth;
using XemPhim.Models;

namespace XemPhim.Controllers
{
    public class AuthController : BaseApiController
    {
        public AuthController()
        {
        }

        public String PostLogin(LoginData data)
        {
            return "Ok";
        }

        public async Task<LoginResult> PostLoginA(LoginData data)
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
            LoginResult result = new LoginResult();
            switch (tokenResult.StatusCode)
            {
                case HttpStatusCode.OK:
                    String username = "";
                    result.Success = true;
                    result.Message = "Đăng nhập thành công";
                    ApplicationUser user = this.dbContext.Users.Where(x => x.UserName == username).First();
                    result.Data = new LoginResultData()
                    {
                        User = UserData.From(user),
                        Token = TokenData.From(new AuthTokenManager(this.dbContext).CreateForUser(user)),
                    };
                    break;
                case HttpStatusCode.BadRequest:
                    result.Message = String.Format("Đăng nhập thất bại với lỗi: {0}", "");
                    break;
                default:
                    result.Message = "Đăng nhập thất bại";
                    break;
            }
            return result;
        }
    }
}
