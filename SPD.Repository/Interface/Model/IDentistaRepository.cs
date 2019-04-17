using SPD.Model.Model;

namespace SPD.Repository.Interface.Model
{
    public interface IDentistaRepository : IRepositoryBase<Dentista>
    {
        void UpdateDentista(Dentista dentista, Usuario usuario);

        void Insert(Dentista dentista);

        void Delete(Dentista dentista);
    }
}
