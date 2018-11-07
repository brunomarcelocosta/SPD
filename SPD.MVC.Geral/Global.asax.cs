using SPD.CrossCutting.Util;
using SPD.MVC.Geral.Global;
using SPD.MVC.Geral.Utilities;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Timers;

namespace SPD.MVC.Geral
{
    public class MvcApplicationBase : System.Web.HttpApplication
    {
        protected T GetInstance<T>() where T : class
        {
            return IoCServer.GetWebInstance<T>();
        }

        protected void RegisterIoC()
        {
            IoCServer.RegisterWebIoC();
        }

        protected void RegisterDatabase()
        {
            //var sistemaService = IoCServer.GetScopedInstance<ISistemaService>();

            //sistemaService.SistemaAtivo();
        }

        protected void RegisterTimer(string module)
        {
            var timer = new Timer(GlobalConstants.General.TimerInterval);

            timer.Elapsed += delegate (object sender, ElapsedEventArgs e)
            {
                try
                {
                    Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "{0} - Elapsed from {1}.", e.SignalTime, module));

                    Job.StartReadyJobs(e.SignalTime.Ticks);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "Exception: \"{0}\" Restarting the timer.", exception.Message));

                    timer.Dispose();

                    this.RegisterTimer(module);
                }
            };

            timer.Start();
        }
    }
}
