using SPD.Model.Model;
using SPD.Model.Util;

namespace SPD.Services.Interface.Model
{
    public interface IDentistaService : IServiceBase<Dentista>
    {
        bool ExisteDentista(Dentista dentista);

        bool ExisteDentistaUsuario(Dentista dentista);

        [Transactional]
        bool Insert(Dentista dentista, Usuario usuario, out string resultado);

        [Transactional]
        bool Update(Dentista dentista, Usuario usuario, out string resultado);

        [Transactional]
        bool Delete(int id, Usuario usuario, out string resultado);
    }
}
