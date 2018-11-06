using Microsoft.AspNet.SignalR;
using System.Diagnostics;

namespace SPD.MVC.Geral.Hubs
{
    public class HubBase : Hub
    {
        public void IsAlive(string message)
        {
            Trace.WriteLine(message);
        }
    }
}
