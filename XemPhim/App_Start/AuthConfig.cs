using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using XemPhim.Data.Seeders;
using XemPhim.Models;

namespace XemPhim
{
    public class AuthConfig
    {
        protected IAppBuilder app;

        public AuthConfig(IAppBuilder app)
        {
            this.app = app;
        }

        public static void Configure(IAppBuilder app)
        {
            AuthConfig config = new AuthConfig(app);
            config.ConfigureOAuth();
        }

        public void ConfigureOAuth()
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/auth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new AuthorizationServerProvider(),
            };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }

    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.CompletedTask;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            if (context.Request.RemoteIpAddress != "::1")
            {
                context.SetError("invalid_scope", "API này chỉ được phép gọi nội bộ");
                return;
            }

            DBContext dbContext = await Seeder.GetDbContextAsync();
            ApplicationSignInManager signInManager = context.OwinContext.Get<ApplicationSignInManager>();

            ApplicationUser user = dbContext.Users.Where(x => x.UserName == context.UserName).FirstOrDefault();
            if (user == null)
            {
                context.SetError("invalid_username", "Tên đăng nhập không hợp lệ");
                return;
            }

            SignInStatus status = await signInManager.PasswordSignInAsync(context.UserName, context.Password, false, false);
            if (status != SignInStatus.Success)
            {
                String errorMessage = String.Format("Tài khoản của bạn đang ở trạng thái {0}", status.ToString());
                if (status == SignInStatus.Failure)
                {
                    errorMessage = "Mật khẩu không hợp lệ";
                }
                context.SetError("invalid_grant", errorMessage);
                return;
            }

            ClaimsIdentity identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            foreach (IdentityUserRole userRole in user.Roles)
            {
                IdentityRole role = dbContext.Roles.Find(userRole.RoleId);
                identity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
            }
            foreach (IdentityUserClaim claim in user.Claims)
            {
                identity.AddClaim(new Claim(claim.ClaimType, claim.ClaimValue));
            }

            context.Validated(identity);
        }
    }
}