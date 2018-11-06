using SimpleInjector;
using SPD.Repository.Interface;
using SPD.Repository.Repository;
using SPD.Services.Interface;
using SPD.Services.Services;

namespace SPD.CrossCutting
{
    public class BootStrapper
    {
        public static void RegisterServices(Container container)
        {

            #region Service

            container.Register(typeof(IServiceBase<>), typeof(ServiceBase<>), Lifestyle.Scoped);

            #endregion

            #region Repository

            container.Register(typeof(IRepositoryBase<>), typeof(RepositoryBase<>), Lifestyle.Scoped);

            #endregion

        }
    }
}
