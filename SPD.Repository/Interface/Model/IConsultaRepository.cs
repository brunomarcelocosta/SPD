using SPD.Model.Model;

namespace SPD.Repository.Interface.Model
{
    public interface IConsultaRepository : IRepositoryBase<Consulta>
    {
        void Insert(Consulta consulta);

        void InsertSQL(Consulta consulta);
    }
}
