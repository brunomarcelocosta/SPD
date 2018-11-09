using SPD.MVC.Geral.Controllers;
using SPD.MVC.Geral.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPD.MVC.PortalWeb.Controllers
{
    public class HomeController : SecurityController
    {
        public ActionResult Index()
        {
            ViewBag.Config = LeituraArquivo.LerArquivoConfig();
            ViewBag.Menu = LeituraArquivo.LerArquivoMenu();
            return this.View();
        }
    }
}