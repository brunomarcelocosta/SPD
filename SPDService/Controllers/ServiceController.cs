using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SPDService.Services;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading;
using DeathByCaptcha;

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

                Client client = (Client)new SocketClient(ConfigurationManager.AppSettings["usernameCaptcha"].ToString(),
                                                          ConfigurationManager.AppSettings["passwordCaptcha"].ToString());

                Captcha captcha = client.Decode(ConfigurationManager.AppSettings["downloadCaptcha"].ToString(), 120);

                if (captcha.Solved && captcha.Correct)
                {
                    Console.WriteLine("CAPTCHA {0}: {1}", captcha.Id, captcha.Text);
                }

                //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("pt-BR");

                //Console.WriteLine("Iniciando a task.");

                //Begin();

                //Service service = new Service();
            }
            catch (System.Exception ex)
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

            var co = new ChromeOptions();
            co.AddArgument("--incognito");

            string chromeDriver = ConfigurationManager.AppSettings["chromedriver"].ToString();

            try
            {
                driverService = ChromeDriverService.CreateDefaultService(chromeDriver);

                driver = new ChromeDriver(driverService, co, TimeSpan.FromSeconds(300));


            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return driver;
        }

        private void GetImageCaptcha(IWebDriver driver)
        {
            var downloadCaptcha = ConfigurationManager.AppSettings["downloadCaptcha"].ToString();
            var downloadSite = ConfigurationManager.AppSettings["downloadSite"].ToString();

            driver.SwitchTo().ParentFrame();

            ITakesScreenshot ssdriver = driver as ITakesScreenshot;
            Screenshot screenshot = ssdriver.GetScreenshot();

            Screenshot tempImage = screenshot;

            tempImage.SaveAsFile(downloadSite, ScreenshotImageFormat.Png);

            IWebElement my_image = driver.FindElement(By.XPath("//img[contains(@src, 'CaptchaImage.aspx')]"));

            Point point = my_image.Location;
            int width = my_image.Size.Width;
            int height = my_image.Size.Height;

            Rectangle section = new Rectangle(point, new Size(width, height));
            Bitmap source = new Bitmap(downloadSite);
            Bitmap final_image = CropImage(source, section);

            final_image.Save(downloadCaptcha);

        }

        public Bitmap CropImage(Bitmap source, Rectangle section)
        {
            Bitmap bmp = new Bitmap(section.Width, section.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);
            return bmp;
        }

        private void GetTextCaptcha(IWebDriver driver)
        {
            driver.FindElement(By.CssSelector("body")).SendKeys(Keys.Control + "t");
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            driver.Navigate().GoToUrl(ConfigurationManager.AppSettings["breakcaptcha"].ToString());

            //login-username
            //login-password

            //driver.SwitchTo().Window(driver.WindowHandles.First());
        }

    }
}
