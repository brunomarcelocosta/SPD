using SPD.MVC.Geral;
using SPD.MVC.Geral.AutoMapper;
using SPD.MVC.Geral.Global;
using SPD.MVC.Geral.Utilities;
using SPD.MVC.PortalWeb.App_Start;
using SPD.MVC.PortalWeb.AutoMapper;
using System;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace SPD.MVC.PortalWeb
{
    public class MvcApplication : MvcApplicationBase
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(new RequestHeaderMapping("Accept", "text/html", StringComparison.InvariantCultureIgnoreCase, true, "application/json"));
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfiguration.RegisterMappings<ViewModelToDomainMappingProfile, DomainToViewModelMappingProfile>();
            SessionStateServer.Register(this.Modules);

            base.RegisterIoC();
            base.RegisterTimer(GlobalConstants.Modules.Web.Name);
            base.RegisterDatabase();


        }
    }
}
