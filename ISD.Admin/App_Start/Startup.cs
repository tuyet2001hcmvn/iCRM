using ISD.Extensions;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
[assembly: OwinStartup(typeof(ISD.Admin.App_Start.Startup))]
namespace ISD.Admin.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Permission/Auth/Login")
            });
        }
    }
}