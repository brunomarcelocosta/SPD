using Microsoft.AspNet.Identity;
using SPD.Model.Model;
using System;
using System.Threading.Tasks;

namespace SPD.MVC.Geral.Utilities
{
    public class UsuarioStore<T> : IUserStore<T> where T : Usuario
    {
        public Task CreateAsync(T user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T user)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
            // throw new NotImplementedException();
        }
    }

}
