using SPD.Model.Model;
using System.Collections.Generic;

namespace SPD.Services.Interface.Model
{
    public interface IHistoricoConsultaService : IServiceBase<HistoricoConsulta>
    {
        List<HistoricoConsulta> Select();
    }
}
