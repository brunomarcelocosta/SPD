using Microsoft.AspNet.SignalR.Hubs;

namespace SPD.MVC.PortalWeb.Hubs
{
    [HubName("sistemaHub")]
    public class SistemaHub : Geral.Hubs.HubBase
    {
        public void Desativar(string url)
        {
            this.Clients.Others.desativar(url);
        }
    }
}