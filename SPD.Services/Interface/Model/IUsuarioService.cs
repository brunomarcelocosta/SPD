using SPD.Model.Model;
using SPD.Model.Util;

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
    }
}
