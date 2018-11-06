using SimpleInjector;
using SimpleInjector.Integration.Web;
using SPD.CrossCutting;

namespace SPD.MVC.Geral.Utilities
{
    public sealed class IoCServer
    {
        public static Container CreateContainer()
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            BootStrapper.RegisterServices(container);

            return container;
        }

        public static T GetInstance<T>() where T : class
        {
            var container = IoCServer.CreateContainer();

            return container.GetInstance<T>();
        }
    }
}
