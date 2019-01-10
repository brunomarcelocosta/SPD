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
                using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                {
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

                    bool existeUser = false;
                    var user = _UsuarioRepository.GetByLogin(auxUsuario.LOGIN);
                    var funcionalidades = _FuncionalidadeRepository.Query().Where(a => a.NOME.Equals("Efetuar Login")).ToList().Count();

                    if (user != null)
                        existeUser = _UsuarioFuncionalidadeRepository.Query().Where(a => a.ID_USUARIO == user.ID && funcionalidades > 0).ToList().Count() > 0 ? true : false;

                    if (existeUser)
                    {
                        usuario = this._UsuarioRepository.GetByLoginSenha(usuario.LOGIN, Usuario.GerarHash(usuario.PASSWORD));

                        if (usuario != null)
                        {
                            if (this._UsuarioRepository.IsAtivo(usuario))
                            {
                                if (!this._UsuarioRepository.IsBloqueado(usuario))
                                {
                                    var conexoes = this._SessaoUsuarioRepository.VerificaConexoesSimultaneas(usuario);

                                    if (conexoes.Count > 0)
                                    {
                                        _SessaoUsuarioRepository.RemoveRange(conexoes);
                                        _SessaoUsuarioRepository.SaveChanges();
                                    }

                                    this._SessaoUsuarioRepository.CriarSessao(usuario, enderecoIP);

                                    this._HistoricoOperacaoRepository.RegistraHistorico("Usuário efetuou login", usuario, Tipo_Operacao.Login, Tipo_Funcionalidades.EfetuarLogin);

                                    usuario = this._UsuarioRepository.ZerarTentativas(usuario);

                                    usuario.ListUsuarioFuncionalidade = _UsuarioFuncionalidadeRepository.Query().Where(a => a.ID_USUARIO == user.ID).ToList();

                                }
                                else
                                {
                                    this._HistoricoOperacaoRepository.RegistraHistorico("Tentativa de acesso de usuário bloqueado.", usuario, Tipo_Operacao.Login, Tipo_Funcionalidades.EfetuarLogin);

                                    throw new Exception("Usuário bloqueado. Contate o administrador.");
                                }
                            }
                            else
                            {
                                this._HistoricoOperacaoRepository.RegistraHistorico("Tentativa de acesso de usuário inativo", usuario, Tipo_Operacao.Login, Tipo_Funcionalidades.EfetuarLogin);

                                usuario = this._UsuarioRepository.IncrementarTentativas(usuario);

                                throw new Exception("Usuário inativo. Contate o administrador.");
                            }
                        }
                        else
                        {
                            usuario = this._UsuarioRepository.GetByLogin(auxUsuario.LOGIN);

                            if (usuario == null)
                            {
                                this._HistoricoOperacaoRepository.RegistraHistoricoSistema("Login Inexistente");

                                throw new Exception("Login e/ou senha inválidos.");
                            }
                            else
                            {
                                this._HistoricoOperacaoRepository.RegistraHistorico("Senha inválida para o login informado", usuario, Tipo_Operacao.Login, Tipo_Funcionalidades.EfetuarLogin);

                                if (!this._UsuarioRepository.IsBloqueado(usuario))
                                {
                                    usuario = this._UsuarioRepository.IncrementarTentativas(usuario);

                                    if (usuario.TENTATIVAS_LOGIN == 3)
                                    {
                                        throw new Exception("Resta apenas uma tentativa incorreta de acesso antes que o login seja bloqueado.");
                                    }
                                    else if (usuario.TENTATIVAS_LOGIN > 3)
                                    {
                                        this._UsuarioRepository.Bloquear(usuario);

                                        this._HistoricoOperacaoRepository.RegistraHistorico("Usuário foi bloqueado", usuario, Tipo_Operacao.Login, Tipo_Funcionalidades.EfetuarLogin);

                                        List<Funcionalidade> recipients = new List<Funcionalidade>();

                                        recipients = _FuncionalidadeRepository.Query().ToList<Funcionalidade>();

                                        foreach (var item in recipients)
                                        {
                                            if (item.NOME.Equals("Efetuar Login"))
                                            {
                                                foreach (var subItem in item.USUARIOS)
                                                {
                                                    try
                                                    {
                                                        new Notificacao().NotificarPorEmail(subItem.EMAIL, "Login do usuário " + subItem.LOGIN + " bloqueado por 3 tentativas inválidas de acesso.", "Bloqueio de usuário", EmailConfiguration.FromEmailSettings());
                                                    }
                                                    catch (SmtpException Exception)
                                                    {
                                                        throw new Exception("Login bloqueado por 3 tentativas inválidas de acesso, porém nem todos os administradores foram notificados.");
                                                    }
                                                }
                                            }
                                        }

                                        throw new Exception("Login bloqueado por 3 tentativas inválidas de acesso.");
                                    }
                                }
                                else
                                {
                                    this._HistoricoOperacaoRepository.RegistraHistorico("Tentativa de acesso de usuário bloqueado.", usuario, Tipo_Operacao.Login, Tipo_Funcionalidades.EfetuarLogin);

                                    throw new Exception("Login e/ou senha inválidos.");
                                }

                                throw new Exception("Login e/ou senha inválidos.");
                            }
                        }
                    }

                    this.SaveChanges(transactionScope);
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
