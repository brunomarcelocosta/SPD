using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Lifestyles;
using System.Reflection;
using System.Web.Mvc;

namespace SPD.CrossCutting.Util
{
    public sealed class IoCServer
    {
        /// <summary>
        /// Método responsável por criar o objeto container
        /// </summary>
        /// <returns></returns>
        public static Container CreateContainer()
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new ThreadScopedLifestyle();

            BootStrapper.RegisterServices(container);

            container.Verify();

            return container;
        }

        /// <summary>
        /// Método responsável por criar o objeto container WEB
        /// </summary>
        /// <returns></returns>
        public static Container CreateWebContainer()
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            BootStrapper.RegisterServices(container);

            container.Verify();

            return container;
        }

        /// <summary>
        /// Método responsável por pegar a instância do objeto container
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetInstance<T>() where T : class
        {
            var container = IoCServer.CreateContainer();

            return container.GetInstance<T>();
        }

        /// <summary>
        /// Método responsável por pegar a instância web  do objeto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetWebInstance<T>() where T : class
        {
            var container = IoCServer.CreateWebContainer();

            return container.GetInstance<T>();
        }

        /// <summary>
        /// Método responsável por pegar o escopo da instância do objeto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetScopedInstance<T>() where T : class
        {
            T instance = default(T);

            var container = IoCServer.CreateContainer();

            using (ThreadScopedLifestyle.BeginScope(container))
            {
                instance = container.GetInstance<T>();
            }

            return instance;
        }

        /// <summary>
        /// Método responsável por registrar o IOC WEB
        /// </summary>
        public static void RegisterWebIoC()
        {
            var container = IoCServer.CreateWebContainer();

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}
