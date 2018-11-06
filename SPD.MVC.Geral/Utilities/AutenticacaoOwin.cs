using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Owin;
using SPD.MVC.Geral.Global;
using System;

namespace SPD.MVC.Geral.Utilities
{
    public static class AutenticacaoOwin
    {
        public static void SetCookieAuth(ref IAppBuilder applicationBuilder)
        {
            applicationBuilder.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                ExpireTimeSpan = TimeSpan.FromMinutes(GlobalConstants.Security.SessionTimeout),
                SlidingExpiration = true,
            });
        }
    }
}
