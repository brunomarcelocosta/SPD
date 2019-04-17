using SPD.Model.Model;
using SPD.Model.Util;
using System.Collections.Generic;

namespace SPD.Services.Interface.Model
{
    public interface IUsuarioService : IServiceBase<Usuario>
    {
        //[Transactional]
        int RedefinirSenha(string login, string emailFrom, string pwdFrom, string enderecoIP);

        bool RedefinirSenha(Usuario usuario, out string novaSenha);

        //[Transactional]
        bool ConfirmarSenha(int ID, string password, out string result);

        //[Transactional]
        bool Desbloquear(Usuario usuarioDesbloqueio, int idUsuarioAtual);

        //[Transactional]
        bool Desconectar(Usuario usuario);

        //[Transactional]
        bool AddNewUser(Usuario usuario, Usuario usuario_logado, List<UsuarioFuncionalidade> usuarioFuncionalidades_ADD, string emailFrom, string pwdFrom, out string resultado);

        //[Transactional]
        bool UpdateUser(Usuario usuario, Usuario usuario_logado, List<UsuarioFuncionalidade> usuarioFuncionalidades_ADD, List<UsuarioFuncionalidade> usuarioFuncionalidades_DEL, out string resultado);

        //[Transactional]
        bool DeleteUser(int id, Usuario usuario_logado, out string resultado);

        List<UsuarioFuncionalidade> HashEntityForUserAndFuncs(List<UsuarioFuncionalidade> usuarioFuncionalidades_ADD, Usuario usuario);

        //[Transactional]
        bool AddUserAndFuncs(List<UsuarioFuncionalidade> usuarioFuncionalidades_ADD, Usuario usuario, Usuario usuario_logado, out string resultado);

        //[Transactional]
        bool DELUserAndFuncs(List<UsuarioFuncionalidade> usuarioFuncionalidades_DEL, Usuario usuario, Usuario usuario_logado, out string resultado);
    }
}
