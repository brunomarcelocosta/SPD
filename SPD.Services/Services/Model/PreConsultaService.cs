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
    public class PreConsultaService : ServiceBase<PreConsulta>, IPreConsultaService
    {
        private readonly IHistoricoOperacaoRepository _HistoricoOperacaoRepository;
        private readonly IPacienteRepository _PacienteRepository;
        private readonly IPreConsultaRepository _PreConsultaRepository;

        public PreConsultaService(IHistoricoOperacaoRepository historicoOperacaoRepository,
                                  IPacienteRepository pacienteRepository,
                                  IPreConsultaRepository preConsultaRepository)
            : base(preConsultaRepository)
        {
            _PacienteRepository = pacienteRepository;
            _PreConsultaRepository = preConsultaRepository;
            _HistoricoOperacaoRepository = historicoOperacaoRepository;
        }
    }
}
