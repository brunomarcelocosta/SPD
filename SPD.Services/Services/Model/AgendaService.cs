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
    public class AgendaService : ServiceBase<Agenda>, IAgendaService
    {
        private readonly IAgendaRepository AgendaRepository;
        private readonly IDentistaRepository DentistaRepository;
        private readonly IHistoricoOperacaoRepository HistoricoOperacaoRepository;
        private readonly IUsuarioRepository UsuarioRepository;

        public AgendaService(IAgendaRepository agendaRepository,
                             IDentistaRepository dentistaRepository,
                             IHistoricoOperacaoRepository historicoOperacaoRepository,
                             IUsuarioRepository usuarioRepository)
            : base(agendaRepository)
        {
            AgendaRepository = agendaRepository;
            DentistaRepository = dentistaRepository;
            HistoricoOperacaoRepository = historicoOperacaoRepository;
            UsuarioRepository = usuarioRepository;
        }
    }
}
