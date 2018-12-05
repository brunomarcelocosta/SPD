using SPD.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Repository.Interface.Model
{
    public interface IUsuarioFuncionalidadeRepository : IRepositoryBase<UsuarioFuncionalidade>
    {
        bool AddList(List<UsuarioFuncionalidade> usuarioFuncionalidades, out string resultado);

        bool AddNewList(List<UsuarioFuncionalidade> usuarioFuncionalidades, Usuario user, out string resultado);

        bool DeleteList(List<UsuarioFuncionalidade> usuarioFuncionalidades, out string resultado);
    }
}
