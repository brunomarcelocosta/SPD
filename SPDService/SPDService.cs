using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using SPDService.Controllers;
using ServiceController = SPDService.Controllers.ServiceController;

namespace SPDService
{
    public partial class SPDService : ServiceBase
    {
        System.Timers.Timer aTimer = new System.Timers.Timer();

        public SPDService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Program._eventLog.WriteEntry("Serviço Iniciado", EventLogEntryType.Information);
            ConfigureTimer();
        }

        protected override void OnStop()
        {
            Program._eventLog.WriteEntry("Serviço Finalizado (Stop)", EventLogEntryType.Information);
        }

        private void ConfigureTimer()
        {
            aTimer.Interval = 1000;
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Start();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                aTimer.Stop();

                aTimer.Interval = 1000 * 60 * int.Parse(ConfigurationManager.AppSettings["ServiceIntervalMinutes"]);

                Program._eventLog.WriteEntry("Inicia Processo Sincronização", EventLogEntryType.Information);
                new ServiceController().StartMethod();
                Program._eventLog.WriteEntry("Finalizado Chamada Processo Sincronização", EventLogEntryType.Information);

                aTimer.Start();
            }
            catch
            {
                aTimer.Start();
            }
        }

    }
}
