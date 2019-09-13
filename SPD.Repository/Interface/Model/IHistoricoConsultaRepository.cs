using SPD.Model.Model;

namespace SPD.Repository.Interface.Model
{
    public interface IHistoricoConsultaRepository : IRepositoryBase<HistoricoConsulta>
    {
        void InsertHistorico(int id_consulta);
    }
}
