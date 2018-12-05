using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Repository.Repository.Model
{
    public class UsuarioFuncionalidadeRepository : RepositoryBase<UsuarioFuncionalidade>, IUsuarioFuncionalidadeRepository
    {

        public bool AddList(List<UsuarioFuncionalidade> usuarioFuncionalidades, out string resultado)
        {
            resultado = "";

            try
            {
                foreach (var item in usuarioFuncionalidades)
                {
                    Add(item);
                }
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
                return false;
            }

            return true;
        }

        public bool AddNewList(List<UsuarioFuncionalidade> usuarioFuncionalidades, Usuario user, out string resultado)
        {
            resultado = "";

            try
            {
                foreach (var item in usuarioFuncionalidades)
                {
                    item.USUARIO = user;

                    Add(item);
                }

                return true;
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
                return false;
            }
        }

        public bool DeleteList(List<UsuarioFuncionalidade> usuarioFuncionalidades, out string resultado)
        {
            resultado = "";

            try
            {
                foreach (var item in usuarioFuncionalidades)
                {
                    var user = GetById(item.ID);
                    Remove(user);
                }
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
                return false;
            }

            return true;
        }

    }
}
