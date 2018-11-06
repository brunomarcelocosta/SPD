using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using SPD.MVC.Geral.Hubs;
using SPD.MVC.Geral.Utilities;

[assembly: OwinStartup(typeof(SPD.MVC.PortalWeb.App_Start.Startup))]
namespace SPD.MVC.PortalWeb.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder applicationBuilder)
        {
            ConfiguracaoAutenticacaoOwin(applicationBuilder);
            ConfiguracaoSignalR(applicationBuilder);
        }

        private static void ConfiguracaoAutenticacaoOwin(IAppBuilder applicationBuilder)
        {
            /*
            applicationBuilder.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                ExpireTimeSpan = TimeSpan.FromMinutes(GlobalConstants.Security.SessionTimeout),
            });
            */

            AutenticacaoOwin.SetCookieAuth(ref applicationBuilder);

            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new SPDUserIdProvider());
        }

        private static void ConfiguracaoSignalR(IAppBuilder applicationBuilder)
        {
            applicationBuilder.UseCors(CorsOptions.AllowAll);
            applicationBuilder.Map("/portalweb", map =>
            {
                map.UseCors(CorsOptions.AllowAll);

                var hubConfiguration = new HubConfiguration()
                {
                    EnableJSONP = true
                };

                map.RunSignalR(hubConfiguration);
            });
        }
    }
}