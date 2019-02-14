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
    public class DentistaService :ServiceBase<Dentista>, IDentistaService
    {
        private readonly IDentistaRepository _DentistaRepository;
        private readonly IHistoricoOperacaoRepository _HistoricoOperacaoRepository;

        public DentistaService(IDentistaRepository dentistaRepository,
                               IHistoricoOperacaoRepository historicoOperacaoRepository)
            :base(dentistaRepository)
        {
            _DentistaRepository = dentistaRepository;
            _HistoricoOperacaoRepository = historicoOperacaoRepository;
        }
    }
}
