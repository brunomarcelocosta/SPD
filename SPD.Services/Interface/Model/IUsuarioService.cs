﻿using SPD.Model.Model;
using SPD.Model.Util;
using System.Collections.Generic;

namespace SPD.Services.Interface.Model
{
    public interface IUsuarioService : IServiceBase<Usuario>
    {
        [Transactional]
        int RedefinirSenha(string login, EmailConfiguration smtpConfiguration, string enderecoIP);

        bool RedefinirSenha(Usuario usuario, out string novaSenha);

        [Transactional]
        bool ConfirmarSenha(int ID, string password, out string result);

        [Transactional]
        bool Desbloquear(Usuario usuarioDesbloqueio, int idUsuarioAtual);

        [Transactional]
        bool Desconectar(Usuario usuario);

        [Transactional]
        bool AddNewUser(Usuario usuario, Usuario usuario_logado, List<UsuarioFuncionalidade> usuarioFuncionalidades_ADD, EmailConfiguration smtpConfiguration, out string resultado);

        [Transactional]
        bool UpdateUser(Usuario usuario, Usuario usuario_logado, List<UsuarioFuncionalidade> usuarioFuncionalidades_ADD, List<UsuarioFuncionalidade> usuarioFuncionalidades_DEL, out string resultado);

        [Transactional]
        bool DeleteUser(int id, Usuario usuario_logado, out string resultado);
    }
}
