using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SPDService.Services;
using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Threading;

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
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("pt-BR");

                Console.WriteLine("Iniciando a task.");

                Begin();

                Service service = new Service();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
        }

        public void Begin()
        {
            // var infos = GetDataProntuario();
            // separar por convenios

            var driver = OpenWebDriver();
            driver.Navigate().GoToUrl(ConfigurationManager.AppSettings["odontoCrown"].ToString());
            GetImageCaptcha(driver);

        }

        private IWebDriver OpenWebDriver()
        {

            var download = ConfigurationManager.AppSettings["download"].ToString();

            var co = new ChromeOptions();
            co.AddUserProfilePreference("download.default_directory", download);
            co.AddArgument("--incognito");

            string chromeDriver = ConfigurationManager.AppSettings["chromedriver"].ToString();

            try
            {
                driverService = ChromeDriverService.CreateDefaultService(chromeDriver);

                driver = new ChromeDriver(driverService, co, TimeSpan.FromSeconds(300));


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return driver;
        }

        private void GetImageCaptcha(IWebDriver driver)
        {
            var download = ConfigurationManager.AppSettings["download"].ToString();

            driver.SwitchTo().ParentFrame();

            var url_img = driver.FindElement(By.XPath("//img[contains(@src, 'CaptchaImage.aspx')]")).GetAttribute("src");

            IWebElement txtID = driver.FindElement(By.XPath("//input[contains(@id, 'txtId')]"));



            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            var url = js.ExecuteScript(@" var text = 'CaptchaImage'; JQuery('img[src*= text]')") as string;

            var base64string = js.ExecuteScript(" Image img = new Image(); img.src = " + url_img + "
                                                    var c = document.createElement('canvas');
                                                    var ctx = c.getContext('2d');
                                                    var img = document.getElementByTagName('img');
                                                    c.height=img.naturalHeight;
                                                    c.width=img.naturalWidth;
                                                    ctx.drawImage(img, 0, 0,img.naturalWidth, img.naturalHeight);
                                                    var base64String = c.toDataURL();
                                                    return base64String",url_img) as string;

            var base64 = base64string.Split(',').Last();
            //using (var stream = new MemoryStream(Convert.FromBase64String(base64)))
            //{
            //    using (var bitmap = new Bitmap(stream))
            //    {
            //        var filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImageName.png");
            //        bitmap.Save(filepath, ImageFormat.Png);
            //    }
            //}

        }

    }
}
