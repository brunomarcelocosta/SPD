using SPD.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Repository.Interface.Model
{
    public interface IHistoricoAutorizacaoPacienteRepository : IRepositoryBase<HistoricoAutorizacaoPaciente>
    {
        void InsertHistorico(int id_paciente, int id_assinatura);
    }
}
