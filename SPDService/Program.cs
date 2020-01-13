using System.Diagnostics;
using System.ServiceProcess;
using ServiceController = SPDService.Controllers.ServiceController;

namespace SPDService
{
    static class Program
    {
        public static EventLog _eventLog;

        static void Main(string[] args)
        {
            _eventLog = CreateApplicationLog();
            _eventLog.WriteEntry("Iniciando Serviço", EventLogEntryType.Information);

            if (args.Length > 0)
            {
                new ServiceController().StartMethod();
            }
            else
            {
                ServiceBase[] ServicesToRun;

                ServicesToRun = new ServiceBase[] { new SPDService() };

                ServiceBase.Run(ServicesToRun);
            }
        }

        private static EventLog CreateApplicationLog()
        {
            if (!EventLog.SourceExists("SPDService"))
            {
                EventLog.CreateEventSource("SPDService", "SPDService");
            }

            return new EventLog
            {
                Source = "SPDService"
            };
        }
    }
}
