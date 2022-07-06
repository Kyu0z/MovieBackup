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
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            DBContext dbContext = await Seeder.GetDbContextAsync();
            ApplicationSignInManager signInManager = context.OwinContext.Get<ApplicationSignInManager>();

            SignInStatus status = await signInManager.PasswordSignInAsync(context.UserName, context.Password, false, false);
            if (status != SignInStatus.Success)
            {
                context.SetError("invalid_grant", String.Format("Login failed with status: {0}", status.ToString()));
                return;
            }

            ClaimsPrincipal claimsPrincipal = signInManager.AuthenticationManager.User;
            if (claimsPrincipal == null)
            {
                context.SetError("invalid_claims", "Invalid session");
                return;
            }
            String username = claimsPrincipal.Identity.Name;
            ApplicationUser user = dbContext.Users.Where(x => x.UserName == username).First();

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", username));
            foreach (IdentityUserRole userRole in user.Roles)
            {
                IdentityRole role = dbContext.Roles.Find(userRole.RoleId);
                identity.AddClaim(new Claim("role", role.Name));
            }
            foreach (IdentityUserClaim claim in user.Claims)
            {
                identity.AddClaim(new Claim(claim.ClaimType, claim.ClaimValue));
            }

            context.Validated(identity);
        }
    }
}