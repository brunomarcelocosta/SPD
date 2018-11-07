using SPD.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Services.Interface.Model
{
    public interface ISessaoUsuarioService : IServiceBase<SessaoUsuario>
    {
        SessaoUsuario GetSessaoByUsuarioID(int usuarioID);

        bool UsuarioConectado(int usuarioID);
    }
}
