using SPD.Model.Model;
using SPD.Model.Util;
using SPD.Repository.Interface.Model;

namespace SPD.Repository.Repository.Model
{
    public class NotificacaoRepository : RepositoryBase<Notificacao>, INotificacaoRepository
    {
        public void NotificarPorEmail(string email, string valor, string assunto, EmailConfiguration smtpConfiguration)
        {
            Notificacao notificacao = new Notificacao();

            notificacao.NotificarPorEmail(email, valor, assunto, smtpConfiguration);
        }
    }
}
