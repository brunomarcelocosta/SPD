using SPD.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Repository.Interface.Model
{
    public interface ISessaoUsuarioRepository : IRepositoryBase<SessaoUsuario>
    {
        void CriarSessao(Usuario usuario, string enderecoIP);

        bool EncerrarSessao(SessaoUsuario sessaoUsuario);

        void EncerrarSessoes(List<SessaoUsuario> sessaoUsuario, Usuario usuario);

        bool UsuarioConectado(int usuarioID);

        SessaoUsuario GetSessaoByUsuarioID(int usuarioID);

        void DesconetarSessaoUsuarios(Usuario usuario, out List<SessaoUsuario> sessoes);

        void DeleteSessao(int id_usuario);

        void InsertSessao(int id_usuario, string ip);
    }
}
