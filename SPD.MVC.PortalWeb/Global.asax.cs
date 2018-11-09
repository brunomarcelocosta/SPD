using SPD.MVC.Geral;
using SPD.MVC.Geral.Global;
using SPD.MVC.Geral.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SPD.MVC.PortalWeb
{
    public class MvcApplication : MvcApplicationBase
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SessionStateServer.Register(this.Modules);


            base.RegisterIoC();
            base.RegisterTimer(GlobalConstants.Modules.Web.Name);
            base.RegisterDatabase();
        }
    }
}
