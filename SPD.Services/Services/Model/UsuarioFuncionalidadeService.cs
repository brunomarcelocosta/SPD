using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using SPD.Services.Interface.Model;

namespace SPD.Services.Services.Model
{
    public class UsuarioFuncionalidadeService : ServiceBase<UsuarioFuncionalidade>, IUsuarioFuncionalidadeService
    {
        private readonly IUsuarioFuncionalidadeRepository _UsuarioFuncionalidadeRepository;

        public UsuarioFuncionalidadeService(IUsuarioFuncionalidadeRepository usuarioFuncionalidadeRepository)
            : base(usuarioFuncionalidadeRepository)
        {
            _UsuarioFuncionalidadeRepository = usuarioFuncionalidadeRepository;
        }
    }
}
