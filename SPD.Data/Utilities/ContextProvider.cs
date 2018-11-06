using SPD.Data.Contexts;
using SPD.Data.Interfaces.Contexts;
using System;

namespace SPD.Data.Utilities
{
    public sealed class ContextProvider
    {
        /// <summary>
        /// Método responsável por criar a instancia do contexto
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>
        public static IDataContext GetDisposableInstance<TContext>() where TContext : BaseContext<TContext>, IDataContext
        {
            return Activator.CreateInstance<TContext>();
        }

        private static IDataContext _Container;

        /// <summary>
        /// Método responsável por pegar a instancia do contexto
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>
        public static IDataContext GetInstance<TContext>() where TContext : BaseContext<TContext>, IDataContext
        {
            if (ContextProvider._Container == null)
            {
                ContextProvider._Container = Activator.CreateInstance<TContext>();
            }

            return _Container;
        }
    }
}
