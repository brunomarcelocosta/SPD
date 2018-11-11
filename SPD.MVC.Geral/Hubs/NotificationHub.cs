using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Transports;
using SPD.CrossCutting.Util;
using SPD.MVC.Geral.Global;
using SPD.MVC.Geral.Utilities;
using SPD.MVC.Geral.ViewModels;
using SPD.Repository.Hubs;
using SPD.Services.Interface.Model;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace SPD.MVC.Geral.Hubs
{
    [HubName("notificationHub")]
    public class NotificationHub : HubBase
    {
        private readonly static ConnectionMapping<string> _Connections = new ConnectionMapping<string>();
        private readonly IUserConnectionRepository _UserConnectionRepository = IoCServer.GetScopedInstance<IUserConnectionRepository>();
        private readonly ISessaoUsuarioService _SessaoUsuarioService = IoCServer.GetScopedInstance<ISessaoUsuarioService>();


        internal static void Notify(Notification notification)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

            if (notification.Countdown > 0)
            {
                switch (notification.For)
                {
                    case NotificationHub.NotificationFor.Nobody:
                        break;

                    case NotificationHub.NotificationFor.All:
                        hubContext.Clients.All.notifyAll(notification.ToString(), Enum.GetName(notification.Type.GetType(), notification.Type).ToLowerInvariant(), notification.UseAnimation, notification.ID.ToString());
                        break;

                    case NotificationHub.NotificationFor.Others:
                        foreach (var connectionID in _Connections.GetConnectionsExcept(notification.UserID.ToString()))
                        {
                            if (String.IsNullOrWhiteSpace(Enum.GetName(notification.Type.GetType(), notification.Type).ToLowerInvariant()))
                            {
                                hubContext.Clients.Client(connectionID).notifyAll(notification.ToString(notification.AlteracaoPerfil), Enum.GetName(NotificationType.INFORMATION.GetType(), NotificationType.INFORMATION).ToLowerInvariant(), notification.UseAnimation, notification.ID.ToString());
                            }
                            else
                            {
                                hubContext.Clients.Client(connectionID).notifyAll(notification.ToString(notification.AlteracaoPerfil), Enum.GetName(notification.Type.GetType(), notification.Type).ToLowerInvariant(), notification.UseAnimation, notification.ID.ToString());
                            }
                        }
                        break;

                    case NotificationHub.NotificationFor.User:
                        foreach (var connectionID in _Connections.GetConnections(notification.UserID.ToString()))
                        {
                            if (String.IsNullOrWhiteSpace(Enum.GetName(notification.Type.GetType(), notification.Type).ToLowerInvariant()))
                            {
                                hubContext.Clients.Client(connectionID).notifyAll(notification.ToString(notification.AlteracaoPerfil), Enum.GetName(NotificationType.INFORMATION.GetType(), NotificationType.INFORMATION).ToLowerInvariant(), notification.UseAnimation, notification.ID.ToString());
                            }
                            else
                            {
                                hubContext.Clients.Client(connectionID).notifyAll(notification.ToString(notification.AlteracaoPerfil), Enum.GetName(notification.Type.GetType(), notification.Type).ToLowerInvariant(), notification.UseAnimation, notification.ID.ToString());
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
            else
            {
                if (notification.AlteracaoPerfil)
                {
                    if (notification.For == NotificationFor.Others)
                    {
                        foreach (var connectionID in _Connections.GetConnectionsExcept(notification.UserID.ToString()))
                        {

                            hubContext.Clients.Client(connectionID).callBack(notification.AlteracaoPerfil);
                            //break;
                        }
                    }
                    else
                    {
                        foreach (var connectionID in _Connections.GetConnections(notification.UserID.ToString()))
                        {

                            hubContext.Clients.Client(connectionID).callBack(notification.AlteracaoPerfil);
                            break;
                        }
                    }

                }
            }
        }

        public void AddUser(string securityToken)
        {
            var authenticationViewModel = Json.Decode<AuthenticationViewModel>(StringCipher.Decrypt(securityToken, GlobalConstants.Security.SessionCipher));

            _Connections.Add(authenticationViewModel.ID.ToString(), this.Context.ConnectionId);
        }

        public void NotifyAll(string notification, string type, bool useAnimation, string id)
        {
            if (String.IsNullOrWhiteSpace(type))
            {
                this.Clients.All.notifyAll(notification, Enum.GetName(NotificationType.INFORMATION.GetType(), NotificationType.INFORMATION).ToLowerInvariant(), useAnimation, id);
            }
            else
            {
                this.Clients.All.notifyAll(notification, type, useAnimation, id);
            }
        }

        public void NotifyOthers(string exceptUserID, string notification, string type, bool useAnimation, string id)
        {
            foreach (var connectionID in _Connections.GetConnectionsExcept(exceptUserID))
            {
                if (String.IsNullOrWhiteSpace(type))
                {
                    this.Clients.Client(connectionID).notifyAll(notification, Enum.GetName(NotificationType.INFORMATION.GetType(), NotificationType.INFORMATION).ToLowerInvariant(), useAnimation, id);
                }
                else
                {
                    this.Clients.Client(connectionID).notifyAll(notification, type, useAnimation, id);
                }
            }
        }

        public void NotifyUser(string userID, string notification, string type, bool useAnimation, string id)
        {
            foreach (var connectionID in _Connections.GetConnections(userID))
            {
                if (String.IsNullOrWhiteSpace(type))
                {
                    this.Clients.Client(connectionID).notifyAll(notification, Enum.GetName(NotificationType.INFORMATION.GetType(), NotificationType.INFORMATION).ToLowerInvariant(), useAnimation, id);
                }
                else
                {
                    this.Clients.Client(connectionID).notifyAll(notification, type, useAnimation, id);
                }
            }
        }

        public enum NotificationType
        {
            SUCCESS,
            INFORMATION,
            WARNING,
            DANGER
        }

        public enum NotificationFor
        {
            Nobody,
            All,
            Others,
            User
        }

        //Remove Inactive Connections
        protected void RemoveInactiveConnections()
        {
            var heartBeat = GlobalHost.DependencyResolver.Resolve<ITransportHeartbeat>();
            var connections = heartBeat.GetConnections().Where(x => x.IsAlive).Select(x => x.ConnectionId).ToList();

            foreach (var userConnection in _UserConnectionRepository.All().FindAll().Where(x => !connections.Contains(x.ConnectionId)))
            {
                _UserConnectionRepository.Remove(userConnection);
            }

        }

        //Check Inactive User Connection
        protected void CheckInactiveUserConnection()
        {
            foreach (var item in _SessaoUsuarioService.GetAll())
            {
                if (_UserConnectionRepository.All().FindAll().Where(x => x.UserId == item.ID_USUARIO).Count() == 0)
                {
                    var usuarioSessao = _SessaoUsuarioService.GetSessaoByUsuarioID(item.ID_USUARIO);
                    _SessaoUsuarioService.EncerrarSessao(usuarioSessao);
                    _SessaoUsuarioService.DesconetarSessaoUsuarios(usuarioSessao.usuario);
                }
            }
        }

        public override Task OnConnected()
        {
            //Remove Inactive Connections
            RemoveInactiveConnections();

            //Only authenticated users
            if (Context.User.Identity.IsAuthenticated)
            {
                var claims = (ClaimsIdentity)Context.User.Identity;
                var id = int.Parse(claims.FindFirst(ClaimTypes.Sid).Value);

                // Get user connections collection
                var userConnection = new UserConnection
                {
                    ConnectionId = Context.ConnectionId,
                    UserId = id,
                    UserName = Context.User.Identity.Name,
                    LastUpdated = DateTime.Now,
                };
                _UserConnectionRepository.AddOrUpdate(userConnection);
            }

            //Check Inactive User Connection
            CheckInactiveUserConnection();

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            //Remove Inactive Connections
            RemoveInactiveConnections();

            var userConnections = _UserConnectionRepository.All().Find(x => x.ConnectionId == Context.ConnectionId);
            foreach (var userConnection in userConnections)
            {
                if (_UserConnectionRepository.All().Find(x => x.UserId == userConnection.UserId).Count() == 0)
                {
                    var sessao = this._SessaoUsuarioService.GetAll().Where(s => s.usuario.ID == userConnection.UserId).FirstOrDefault();
                    if (sessao != null)
                    {
                        var usuarioSessao = _SessaoUsuarioService.GetSessaoByUsuarioID(userConnection.UserId);
                        _SessaoUsuarioService.EncerrarSessao(usuarioSessao);
                    }
                }
            }

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            //Only authenticated users
            if (Context.User.Identity.IsAuthenticated)
            {
                var userConnection = _UserConnectionRepository.All().FindOne(x => x.ConnectionId == Context.ConnectionId);
                if (userConnection != null)
                {
                    userConnection.LastUpdated = DateTime.UtcNow;
                    _UserConnectionRepository.AddOrUpdate(userConnection);
                }
                else
                {
                    userConnection = new UserConnection
                    {
                        ConnectionId = Context.ConnectionId,
                        UserName = Context.User.Identity.Name,
                        LastUpdated = DateTime.Now,
                    };
                    _UserConnectionRepository.AddOrUpdate(userConnection);
                }
            }

            return base.OnReconnected();
        }
    }
}
