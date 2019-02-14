using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using SPD.Services.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Services.Services.Model
{
    public class HistoricoConsultaService : ServiceBase<HistoricoConsulta>, IHistoricoConsultaService
    {
        private readonly IHistoricoConsultaRepository _HistoricoConsultaRepository;

        public HistoricoConsultaService(IHistoricoConsultaRepository historicoConsultaRepository)
            : base(historicoConsultaRepository)
        {
            _HistoricoConsultaRepository = historicoConsultaRepository;
        }
    }
}
