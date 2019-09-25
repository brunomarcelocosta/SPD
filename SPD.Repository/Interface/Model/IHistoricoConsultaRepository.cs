using SPD.Model.Model;
using System.Collections.Generic;

namespace SPD.Repository.Interface.Model
{
    public interface IHistoricoConsultaRepository : IRepositoryBase<HistoricoConsulta>
    {
        void InsertHistorico(int id_consulta);

        List<HistoricoConsulta> Select();
    }
}
