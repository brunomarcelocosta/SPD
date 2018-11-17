using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using SPD.Services.Interface.Model;

namespace SPD.Services.Services.Model
{
    public class FuncionalidadeService : ServiceBase<Funcionalidade>, IFuncionalidadeService
    {
        private readonly IFuncionalidadeRepository _FuncionalidadeRepository;

        public FuncionalidadeService(IFuncionalidadeRepository funcionalidadeRepository) : base(funcionalidadeRepository)
        {
            _FuncionalidadeRepository = funcionalidadeRepository;
        }
    }
}
