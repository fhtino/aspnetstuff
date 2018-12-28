using System;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;

[assembly: OwinStartup(typeof(OwinAuthSimple.Startup))]

namespace OwinAuthSimple
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // --- Cookie authentication ---
            var cookieOptions = new CookieAuthenticationOptions
            {
                LoginPath = new PathString("/login.aspx"),
                ExpireTimeSpan = new TimeSpan(1, 0, 0),
                CookieName = "MyAuth"
            };
            app.UseCookieAuthentication(cookieOptions);
            app.SetDefaultSignInAsAuthenticationType(cookieOptions.AuthenticationType);

            // --- Google authentication ---
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
            {
                ClientId = ConfigurationManager.AppSettings["GoogleClientID"],
                ClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"],
                Provider = new GoogleOAuth2AuthenticationProvider()
                {
                    OnAuthenticated = context =>
                    {
                        context.Identity.AddClaim(new Claim(MyConstants.MyClaimdProviderID, "Google"));
                        return Task.FromResult<object>(null);
                    }
                }
            });

            // --- add roles ---
            app.Use(SimpleAddRoles.AddRoleAndSID)                 // <--- add my roles & information
               .UseStageMarker(PipelineStage.PostAuthenticate);   // <--- very important. Roles must be added after authentication.
        }

    }
}
