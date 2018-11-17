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
    public class HistoricoOperacaoService : ServiceBase<HistoricoOperacao>, IHistoricoOperacaoService
    {
        private readonly IHistoricoOperacaoRepository _HistoricoOperacaoRepository;

        public HistoricoOperacaoService(IHistoricoOperacaoRepository historicoOperacaoRepository)
            : base(historicoOperacaoRepository)
        {
            _HistoricoOperacaoRepository = historicoOperacaoRepository;
        }
    }
}
