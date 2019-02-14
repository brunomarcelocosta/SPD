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
    public class ConsultaService : ServiceBase<Consulta>, IConsultaService
    {
        private readonly IConsultaRepository _ConsultaRepository;
        private readonly IDentistaRepository _DentistaRepository;
        private readonly IHistoricoOperacaoRepository _HistoricoOperacaoRepository;
        private readonly IHistoricoConsultaRepository _HistoricoConsultaRepository;
        private readonly IPreConsultaRepository _PreConsultaRepository;

        public ConsultaService(IConsultaRepository consultaRepository,
                               IDentistaRepository dentistaRepository,
                               IHistoricoOperacaoRepository historicoOperacaoRepository,
                               IHistoricoConsultaRepository historicoConsultaRepository,
                               IPreConsultaRepository preConsultaRepository)
            : base(consultaRepository)
        {
            _ConsultaRepository = consultaRepository;
            _DentistaRepository = dentistaRepository;
            _HistoricoOperacaoRepository = historicoOperacaoRepository;
            _HistoricoConsultaRepository = historicoConsultaRepository;
            _PreConsultaRepository = preConsultaRepository;

        }
    }
}
