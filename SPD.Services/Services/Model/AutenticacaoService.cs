using SPD.Model.Model;
using SPD.Model.Util;
using SPD.Repository.Interface.Model;
using SPD.Services.Interface.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static SPD.Model.Enums.SPD_Enums;

namespace SPD.Services.Services.Model
{
    public class AutenticacaoService : ServiceBase<Usuario>, IAutenticacaoService
    {
        private readonly IUsuarioRepository _UsuarioRepository;
        private readonly ISessaoUsuarioRepository _SessaoUsuarioRepository;
        private readonly IHistoricoOperacaoRepository _HistoricoOperacaoRepository;
        private readonly IFuncionalidadeRepository _FuncionalidadeRepository;
        private readonly IUsuarioFuncionalidadeRepository _UsuarioFuncionalidadeRepository;

        public AutenticacaoService(IUsuarioRepository usuarioRepository, ISessaoUsuarioRepository sessaoUsuarioRepository,
                                   IHistoricoOperacaoRepository historicoOperacaoRepository, IFuncionalidadeRepository funcionalidadeRepository,
                                   IUsuarioFuncionalidadeRepository usuarioFuncionalidadeRepository)
            : base(usuarioRepository)
        {
            _UsuarioRepository = usuarioRepository;
            _SessaoUsuarioRepository = sessaoUsuarioRepository;
            _HistoricoOperacaoRepository = historicoOperacaoRepository;
            _FuncionalidadeRepository = funcionalidadeRepository;
            _UsuarioFuncionalidadeRepository = usuarioFuncionalidadeRepository;
        }

        public bool AutenticarUsuario(ref Usuario usuario, string enderecoIP, string autenticacaoUrl, out string resultados)
        {
            try
            {
                //using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                // {
                resultados = String.Empty;
                string auxLogin = usuario.LOGIN;

                Usuario auxUsuario = new Usuario()
                {
                    LOGIN = auxLogin
                };

                if (usuario.LOGIN == null || usuario.PASSWORD == null)
                {
                    throw new Exception("Login e Senha são obrigatórios");
                }

                var user = _UsuarioRepository.QueryAsNoTracking().Where(a => a.LOGIN.ToUpper().Equals(auxUsuario.LOGIN.ToUpper())).FirstOrDefault();

                var funcionalidades = _FuncionalidadeRepository.QueryAsNoTracking().Where(a => a.NOME.Equals("Efetuar Login")).FirstOrDefault();

                if (user == null)
                {
                    throw new Exception("Não existe este usuário no sistema.");
                }

                var existeUser = _UsuarioFuncionalidadeRepository.Query().Where(a => a.ID_USUARIO == user.ID && a.ID_FUNCIONALIDADE == funcionalidades.ID).ToList().Count() > 0 ? true : false;

                if (existeUser)
                {
                    var senha_hash = Usuario.GerarHash(usuario.PASSWORD);

                    var senha_correta = user.PASSWORD.Equals(senha_hash) ? true : false;

                    if (senha_correta)
                    {
                        if (user.IsATIVO)
                        {
                            if (!user.IsBLOQUEADO)
                            {
                                var conexoes = this._SessaoUsuarioRepository.QueryAsNoTracking().Where(sessaoUsuario => sessaoUsuario.ID_USUARIO.Equals(user.ID)).ToList();

                                if (conexoes.Count > 0)
                                {
                                    _SessaoUsuarioRepository.DeleteSessao(user.ID);
                                }

                                this._SessaoUsuarioRepository.InsertSessao(user.ID, enderecoIP);

                                this._HistoricoOperacaoRepository.Insert("Usuário efetuou login", user, Tipo_Operacao.Login, Tipo_Funcionalidades.EfetuarLogin);

                                usuario = user;

                                var list = _UsuarioFuncionalidadeRepository.Query().Where(a => a.ID_USUARIO == user.ID).ToList();

                                usuario.ListUsuarioFuncionalidade = list;

                                usuario.FuncionalidadesUsuarioIDs = list.Select(a => a.ID_FUNCIONALIDADE).ToList();
                                usuario.FuncionalidadesUsuarioNomes = list.Select(a => a.Funcionalidade.NOME).ToList();
                            }

                            else
                            {
                                this._HistoricoOperacaoRepository.Insert("Tentativa de acesso de usuário bloqueado.", usuario, Tipo_Operacao.Login, Tipo_Funcionalidades.EfetuarLogin);

                                throw new Exception("Usuário bloqueado. Contate o administrador.");
                            }
                        }
                        else
                        {
                            this._HistoricoOperacaoRepository.Insert("Tentativa de acesso de usuário inativo", usuario, Tipo_Operacao.Login, Tipo_Funcionalidades.EfetuarLogin);

                            throw new Exception("Usuário inativo. Contate o administrador.");
                        }
                    }
                    else
                    {
                        throw new Exception("Senha inválida.");
                    }

                }
                else
                {
                    // this._HistoricoOperacaoRepository.InsertHistoricoSistema("Usuário não possui esta funcionalidade.");

                    throw new Exception("Usuário não possui esta funcionalidade.");
                }

            }
            catch (Exception exception)
            {
                resultados = exception.Message;

                return false;
            }

            return true;
        }

    }
}
