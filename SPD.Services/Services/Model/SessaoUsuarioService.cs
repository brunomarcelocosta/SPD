using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using SPD.Services.Interface.Model;
using System;
using System.Collections.Generic;

namespace SPD.Services.Services.Model
{
    public class SessaoUsuarioService : ServiceBase<SessaoUsuario>, ISessaoUsuarioService
    {
        private readonly ISessaoUsuarioRepository _SessaoUsuarioRepository;
        private readonly IHistoricoOperacaoRepository _HistoricoOperacaoRepository;
        private readonly IUsuarioService _UsuarioService;

        public SessaoUsuarioService(ISessaoUsuarioRepository sessaoUsuarioRepository, IUsuarioService usuarioService,
                                    IHistoricoOperacaoRepository historicoOperacaoRepository)
            : base(sessaoUsuarioRepository)
        {
            _SessaoUsuarioRepository = sessaoUsuarioRepository;
            _UsuarioService = usuarioService;
            _HistoricoOperacaoRepository = historicoOperacaoRepository;
        }

        public SessaoUsuario GetSessaoByUsuarioID(int usuarioID)
        {
            return _SessaoUsuarioRepository.GetSessaoByUsuarioID(usuarioID);
        }

        public bool UsuarioConectado(int usuarioID)
        {
            return this._SessaoUsuarioRepository.UsuarioConectado(usuarioID);
        }

        public bool EncerrarSessao(SessaoUsuario sessaoUsuario)
        {
            sessaoUsuario.EncerrarSessao(this.Context as Context);

            return this._SessaoUsuarioRepository.EncerrarSessao(sessaoUsuario);
        }

        public void DesconetarSessaoUsuarios(Usuario usuario)
        {
            List<SessaoUsuario> sessoes = new List<SessaoUsuario>();
            this._SessaoUsuarioRepository.DesconetarSessaoUsuarios(usuario, out sessoes);

            foreach (var sessao in sessoes)
            {
                this._HistoricoOperacaoRepository.RegistraHistoricoSistema(String.Format("Usuário {0} desconectado pelo sistema", this._UsuarioService.GetById(sessao.UsuarioID).Nome));
                this._HistoricoOperacaoRepository.SaveChanges();
            }
        }
    }
}
