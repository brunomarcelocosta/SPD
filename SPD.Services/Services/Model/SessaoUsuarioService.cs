using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using SPD.Services.Interface.Model;

namespace SPD.Services.Services.Model
{
    public class SessaoUsuarioService : ServiceBase<SessaoUsuario>, ISessaoUsuarioService
    {
        private readonly ISessaoUsuarioRepository _SessaoUsuarioRepository;

        public SessaoUsuarioService(ISessaoUsuarioRepository sessaoUsuarioRepository) : base(sessaoUsuarioRepository)
        {
            _SessaoUsuarioRepository = sessaoUsuarioRepository;
        }

        public SessaoUsuario GetSessaoByUsuarioID(int usuarioID)
        {
            return _SessaoUsuarioRepository.GetSessaoByUsuarioID(usuarioID);
        }

        public bool UsuarioConectado(int usuarioID)
        {
            return this._SessaoUsuarioRepository.UsuarioConectado(usuarioID);
        }
    }
}
