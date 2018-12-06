using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using SPD.Services.Interface.Model;

namespace SPD.Services.Services.Model
{
    public class PacienteService : ServiceBase<Paciente>, IPacienteService
    {
        private readonly IPacienteRepository _PacienteRepository;
        private readonly IHistoricoOperacaoRepository _HistoricoOperacaoRepository;

        public PacienteService(IPacienteRepository pacienteRepository,
                               IHistoricoOperacaoRepository historicoOperacaoRepository)
            : base(pacienteRepository)
        {
            _PacienteRepository = pacienteRepository;
            _HistoricoOperacaoRepository = historicoOperacaoRepository;
        }
    }
}
