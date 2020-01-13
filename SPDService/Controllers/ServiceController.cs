using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SPDService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SPDService.Controllers
{
    public class ServiceController
    {
        public IWebDriver driver;
        private ChromeDriverService driverService;

        public void StartMethod()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("pt-BR");
                // site :  https://seguro10.iphotel.com.br/crownodonto/novosite/dentistas.html

                Service service = new Service();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
