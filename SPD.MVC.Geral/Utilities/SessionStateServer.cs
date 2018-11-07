using SPD.MVC.Geral.Global;
using System;
using System.Reflection;
using System.ServiceProcess;
using System.Web;
using System.Web.SessionState;

namespace SPD.MVC.Geral.Utilities
{
    public sealed class SessionStateServer
    {
        private const string SESSIONSTATESERVERNAME = "aspnet_state";

        public static void Register(HttpModuleCollection modules)
        {
            try
            {
                if (GlobalConstants.Security.UseSessionStateServer)
                {
                    if (!String.IsNullOrEmpty(GlobalConstants.Security.AuthenticationSession))
                    {
                        foreach (string moduleName in modules)
                        {
                            IHttpModule module = modules[moduleName];

                            SessionStateModule sessionStateModule = module as SessionStateModule;

                            if (sessionStateModule != null)
                            {
                                FieldInfo storeInfo = typeof(SessionStateModule).GetField("_store", BindingFlags.Instance | BindingFlags.NonPublic);

                                SessionStateStoreProviderBase store = (SessionStateStoreProviderBase)storeInfo.GetValue(sessionStateModule);

                                if (store == null) // In IIS7 Integrated mode, module.Init() is called later
                                {
                                    FieldInfo runtimeInfo = typeof(HttpRuntime).GetField("_theRuntime", BindingFlags.Static | BindingFlags.NonPublic);

                                    HttpRuntime runtime = (HttpRuntime)runtimeInfo.GetValue(null);

                                    FieldInfo applicationInfo = typeof(HttpRuntime).GetField("_appDomainAppId", BindingFlags.Instance | BindingFlags.NonPublic);

                                    applicationInfo.SetValue(runtime, GlobalConstants.Security.AuthenticationSession);
                                }
                                else
                                {
                                    Type storeType = store.GetType();

                                    if (storeType.Name.Equals("OutOfProcSessionStateStore"))
                                    {
                                        FieldInfo uribaseInfo = storeType.GetField("s_uribase", BindingFlags.Static | BindingFlags.NonPublic);

                                        uribaseInfo.SetValue(storeType, GlobalConstants.Security.AuthenticationSession);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public static void Start()
        {
            var sessionStateServer = new ServiceController(SessionStateServer.SESSIONSTATESERVERNAME, Environment.MachineName);

            if (sessionStateServer.Status == ServiceControllerStatus.Stopped || sessionStateServer.Status != ServiceControllerStatus.StopPending)
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(10000);

                sessionStateServer.Start();

                sessionStateServer.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
        }

        public static void Stop()
        {
            var sessionStateServer = new ServiceController(SessionStateServer.SESSIONSTATESERVERNAME, Environment.MachineName);

            if (sessionStateServer.Status == ServiceControllerStatus.Running)
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(10000);

                sessionStateServer.Stop();

                sessionStateServer.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            }
        }

        public static void Restart()
        {
            SessionStateServer.Stop();

            SessionStateServer.Start();
        }

        public static ServiceControllerStatus Status()
        {
            var sessionStateServer = new ServiceController(SessionStateServer.SESSIONSTATESERVERNAME, Environment.MachineName);

            return sessionStateServer.Status;
        }
    }
}
