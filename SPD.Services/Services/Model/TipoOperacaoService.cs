using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using SPD.Services.Interface.Model;

namespace SPD.Services.Services.Model
{
    public class TipoOperacaoService : ServiceBase<TipoOperacao>, ITipoOperacaoService
    {
        private readonly ITipoOperacaoRepository _TipoOperacaoRepository;

        public TipoOperacaoService(ITipoOperacaoRepository tipoOperacaoRepository)
            : base(tipoOperacaoRepository)
        {
            _TipoOperacaoRepository = tipoOperacaoRepository;
        }
    }
}
