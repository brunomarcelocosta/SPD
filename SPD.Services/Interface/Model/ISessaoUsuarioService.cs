using SPD.Model.Model;
using SPD.Model.Util;
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

        bool EncerrarSessao(SessaoUsuario sessaoUsuario);

        [Transactional]
        bool EncerrarSessao(int usuarioID, string valor);

        void DesconetarSessaoUsuarios(Usuario usuario);
    }
}
