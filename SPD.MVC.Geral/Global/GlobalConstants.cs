using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SPD.MVC.Geral.Global
{
    public sealed class GlobalConstants
    {
        public const string ControllerKey = "controller";
        public const string ActionKey = "action";

        public sealed class General
        {
            // conteúdo da const Domain removido em 07/05/2018: Url será lida do arquivo Solution Items -> SGTAN.Modulos.json -> tag Url
            public const string Domain = "";
            public const int PaginationLimit = 50;
            public const int TimerInterval = 1000;
            public const string Version = "1.0.0.0";
            public const string ContentType = "text/plain";
            public const string LocalhostIPv6 = "::1";
            public const string LocalhostIPv4 = "127.0.0.1";
            public const string CurrencyMinRange = "0";
            public const string CurrencyMaxRange = "999999999999";
            public const string DateFormat = "{0:dd/MM/yyyy}";
            public const string DateTimeFormat = "{0:dd/MM/yyyy HH:mm:ss}";
            public const string NotificationTimeFormat = @"mm\:ss";
            public const string DateFilterWidgetFormat = "dd.mm.yyyy";
            public const string SaveButtonStyle = "btn btn-primary";
            public const string CancelButtonStyle = "btn btn-secondary";
            public const string PrintButtonStyle = "btn btn-primary";
            public const string ExportButtonStyle = "btn btn-primary";
            public const string EditButtonStyle = "btn btn-primary";
            public const string DeleteButtonStyle = "btn btn-danger";
            public const string DetailButtonStyle = "btn btn-primary";
        }

        public sealed class Security
        {
            public const bool UseSessionStateServer = true;
            public const string AuthenticationSession = "AuthenticationSession";
            public const int SessionTimeout = 120; // In Minutes
            public const string SessionCipher = "!@xccvs12395Jh##$";
        }

        public sealed class Home
        {
            public const string Controller = "Home";
            public const string HomeAction = "Index";

            public static string HomeUrl
            {
                get
                {
                    return String.Format("{0}/{1}/{2}", GlobalConstants.Modules.Web.Url, Home.Controller, Home.HomeAction);
                }
            }
        }

        public sealed class Login
        {
            public const string Controller = "Login";
            public const string AuthenticateAction = "AutenticarLogin";
            public const string AuthenticatedAction = "LoginAutenticado";
            public const string AuthenticationStatusAction = "Status";
            public const string UnauthenticateAction = "InvalidarLogin";

            public static string AuthenticateUrl
            {
                get
                {
                    return String.Format("{0}/{1}/{2}", GlobalConstants.Modules.Web.Url, Login.Controller, Login.AuthenticateAction);
                }
            }

            public static string UnauthenticateUrl
            {
                get
                {
                    return String.Format("{0}/{1}/{2}", GlobalConstants.Modules.Web.Url, Login.Controller, Login.UnauthenticateAction);
                }
            }
        }

        public sealed class RedefinePassword
        {
            public const string Controller = "AlterarSenha";
            public const string RedefinePasswordAction = "Alterar";

            public static string RedefinePasswordUrl
            {
                get
                {
                    return String.Format("{0}/{1}/{2}", GlobalConstants.Modules.Web.Url, RedefinePassword.Controller, RedefinePassword.RedefinePasswordAction);
                }
            }
        }

        public sealed class ConfirmRedefinePassword
        {
            public const string Controller = "AlterarSenha";
            public const string ConfirmRedefinePasswordAction = "Confirmar";

            public static string ConfirmRedefinePasswordUrl
            {
                get
                {
                    return String.Format("{0}/{1}/{2}", GlobalConstants.Modules.Web.Url, ConfirmRedefinePassword.Controller, ConfirmRedefinePassword.ConfirmRedefinePasswordAction);
                }
            }
        }

        public sealed class Modules
        {
            public static string Action(string baseUrl, string controller, string action)
            {
                return String.Format(CultureInfo.InvariantCulture, "{0}/{1}/{2}", baseUrl, controller, action);
            }

            public static string Action(string baseUrl, string controller, string action, string parameter)
            {
                return String.Format(CultureInfo.InvariantCulture, "{0}/{1}/{2}/{3}", baseUrl, controller, action, parameter);
            }

            public sealed class Web
            {
                public const string Name = "Web";
                public const string Port = "59916";

                public static string Url
                {
                    get
                    {
                        var url = HttpContext.Current.Request.Url;

                        return String.Format("{0}://{1}:{2}", url.Scheme, url.Host, Web.Port);
                    }
                }
            }
        }
    }
}
