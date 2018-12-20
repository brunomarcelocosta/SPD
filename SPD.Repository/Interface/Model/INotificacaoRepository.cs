using SPD.Model.Model;
using SPD.Model.Util;

namespace SPD.Repository.Interface.Model
{
    public interface INotificacaoRepository : IRepositoryBase<Notificacao>
    {
        void NotificarPorEmail(string email, string valor, string assunto, string emailFrom, string pwdFrom);
    }
}
