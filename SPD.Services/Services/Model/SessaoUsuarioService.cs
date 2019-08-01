using SPD.Model.Model;
using SPD.Model.Util;
using SPD.Repository.Interface.Model;
using SPD.Services.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using static SPD.Model.Enums.SPD_Enums;

namespace SPD.Services.Services.Model
{
    public class SessaoUsuarioService : ServiceBase<SessaoUsuario>, ISessaoUsuarioService
    {
        private readonly ISessaoUsuarioRepository _SessaoUsuarioRepository;
        private readonly IHistoricoOperacaoRepository _HistoricoOperacaoRepository;
        private readonly IUsuarioRepository _UsuarioRepository;

        public SessaoUsuarioService(ISessaoUsuarioRepository sessaoUsuarioRepository, IUsuarioRepository usuarioRepository,
                                    IHistoricoOperacaoRepository historicoOperacaoRepository)
            : base(sessaoUsuarioRepository)
        {
            _SessaoUsuarioRepository = sessaoUsuarioRepository;
            _UsuarioRepository = usuarioRepository;
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

        public bool EncerrarSessao(int usuarioID, string valor)
        {
            bool result;
            try
            {
                //using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                //{
                var usuario = this._UsuarioRepository.GetById(usuarioID);

                this._HistoricoOperacaoRepository.Insert(valor, usuario, Tipo_Operacao.Logoff, Tipo_Funcionalidades.EfetuarLogof);
                var sessao = this._SessaoUsuarioRepository.GetAll().Where(s => s.usuario.ID == usuarioID).FirstOrDefault();
                if (sessao == null)
                    return false;

                result = this._SessaoUsuarioRepository.EncerrarSessao(sessao);

                //  this.SaveChanges(transactionScope);
                //}

                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void DesconetarSessaoUsuarios(Usuario usuario)
        {
            List<SessaoUsuario> sessoes = new List<SessaoUsuario>();
            this._SessaoUsuarioRepository.DesconetarSessaoUsuarios(usuario, out sessoes);

            foreach (var sessao in sessoes)
            {
                var valor = this._UsuarioRepository.GetById(sessao.ID_USUARIO).NOME;

                this._HistoricoOperacaoRepository.InsertHistoricoSistema(valor);
            }
        }
    }
}
