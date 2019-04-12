using SPD.Model.Model;

namespace SPD.Repository.Interface.Model
{
    public interface IAssinaturaRepository : IRepositoryBase<Assinatura>
    {
        Assinatura GetAssinatura(Assinatura assinatura, bool existe);
    }
}
