using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Repository.Repository.Model
{
    public class HistoricoAutorizacaoPacienteRepository : RepositoryBase<HistoricoAutorizacaoPaciente>, IHistoricoAutorizacaoPacienteRepository
    {
        public void InsertHistorico(HistoricoAutorizacaoPaciente historico)
        {
            Add(historico);
            SaveChanges();
        }
    }
}
