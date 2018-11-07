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


        public AutenticacaoService(IUsuarioRepository usuarioRepository, ISessaoUsuarioRepository sessaoUsuarioRepository)
            : base(usuarioRepository)
        {
            _UsuarioRepository = usuarioRepository;
            _SessaoUsuarioRepository = sessaoUsuarioRepository;
        }

        public bool AutenticarUsuario(ref Usuario usuario, string enderecoIP, string autenticacaoUrl, out string resultados)
        {
            try
            {
                using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                {
                    resultados = String.Empty;
                    string auxLogin = usuario.Login;

                    Usuario auxUsuario = new Usuario()
                    {
                        Login = auxLogin
                    };

                    if (usuario.Login == null || usuario.Password == null)
                    {
                        throw new Exception("Login e Senha são obrigatórios");
                    }

                    if ((this._UsuarioRepository.GetByLogin(auxUsuario.Login) != null && this._UsuarioRepository.GetByLogin(auxUsuario.Login).ListUsuarioFuncionalidade.Where(u => u.FUNCIONALIDADE.Nome == "Login").Any()))
                    {
                        usuario = this._UsuarioRepository.GetByLoginSenha(usuario.Login, Usuario.GerarHash(usuario.Password));

                        if (usuario != null)
                        {
                            if (this._UsuarioRepository.IsAtivo(usuario))
                            {
                                if (!this._UsuarioRepository.IsBloqueado(usuario))
                                {
                                    var conexoes = this._SessaoUsuarioRepository.VerificaConexoesSimultaneas(usuario);

                                    if (conexoes.Count > 0)
                                    {
                                        var usuarioID = Usuario.EncryptID(Convert.ToString(usuario.ID));
                                        var url = String.Format(CultureInfo.InvariantCulture, "{0}/Autenticacao/Logout/?k={1}", autenticacaoUrl, usuarioID);

                                        new Notificacao().NotificarPorEmail(usuario.Email, string.Format("Tentativa de acesso simultâneo do Login {0}. Usuário já está conectado em outra estação de trabalho.<br /><br />Clique <a href='{1}'>aqui</a> para forçar seu logout da outra estação ou acesse o link <a href='{1}'>{1}</a> pelo seu navegador.", usuario.Login, url), "Acesso simultâneo identificado", EmailConfiguration.FromEmailSettings());

                                        // 6.2.Sistema registra o acesso simultâneo [UC02.22 – Registrar Histórico de Operação de Usuário];
                                        this._HistoricoOperacaoService.RegistraHistorico("Usuário conectado em outra estação de trabalho", usuario, Tipo_Operacao.Logoff, Tipo_Funcionalidades.EfetuarLogof);

                                        // 6.3.Sistema exibe a mensagem informando que o usuário já está conectado em outra estação de trabalho;
                                        throw new Exception("Usuário já está conectado em outra estação de trabalho.");
                                    }
                                    else
                                    {
                                        this._SessaoUsuarioRepository.CriarSessao(usuario, enderecoIP);

                                        this._HistoricoOperacaoService.RegistraHistorico("Usuário efetuou login", usuario, Tipo_Operacao.Login, Tipo_Funcionalidades.EfetuarLogin);

                                        usuario = this._UsuarioRepository.ZerarTentativas(usuario);
                                    }
                                }
                                else
                                {
                                    this._HistoricoOperacaoService.RegistraHistorico("Tentativa de acesso de usuário bloqueado.", usuario, Tipo_Operacao.Login, Tipo_Funcionalidades.EfetuarLogin);

                                    throw new Exception("Usuário bloqueado. Contate o administrador.");
                                }
                            }
                            else
                            {
                                this._HistoricoOperacaoService.RegistraHistorico("Tentativa de acesso de usuário inativo", usuario, Tipo_Operacao.Login, Tipo_Funcionalidades.EfetuarLogin);

                                usuario = this._UsuarioRepository.IncrementarTentativas(usuario);

                                throw new Exception("Usuário inativo. Contate o administrador.");
                            }
                        }
                        else
                        {
                            usuario = this._UsuarioRepository.GetByLogin(auxUsuario.Login);

                            if (usuario == null)
                            {
                                this._HistoricoOperacaoService.RegistraHistoricoSistema("Login Inexistente");

                                throw new Exception("Login e/ou senha inválidos.");
                            }
                            else
                            {
                                this._HistoricoOperacaoService.RegistraHistorico("Senha inválida para o login informado", usuario, Tipo_Operacao.Login, Tipo_Funcionalidades.EfetuarLogin);

                                if (!this._UsuarioRepository.IsBloqueado(usuario))
                                {
                                    usuario = this._UsuarioRepository.IncrementarTentativas(usuario);

                                    if (usuario.TentativasLogin == 3)
                                    {
                                        throw new Exception("Resta apenas uma tentativa incorreta de acesso antes que o login seja bloqueado.");
                                    }
                                    else if (usuario.TentativasLogin > 3)
                                    {
                                        this._UsuarioRepository.Bloquear(usuario);

                                        this._HistoricoOperacaoService.RegistraHistorico("Usuário foi bloqueado", usuario, Tipo_Operacao.Login, Tipo_Funcionalidades.EfetuarLogin);

                                        List<Funcionalidade> recipients = new List<Funcionalidade>();

                                        recipients = _FuncionalidadeRepoistory.Query().ToList<Funcionalidade>();

                                        foreach (var item in recipients)
                                        {
                                            if (item.Nome.Equals("Login"))
                                            {
                                                foreach (var subItem in item.Usuarios)
                                                {
                                                    try
                                                    {
                                                        new Notificacao().NotificarPorEmail(subItem.Email, "Login do usuário " + subItem.Login + " bloqueado por 3 tentativas inválidas de acesso.", "Bloqueio de usuário", EmailConfiguration.FromEmailSettings());
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
                                    this._HistoricoOperacaoRepository.RegistraHistorico("Tentativa de acesso de usuário bloqueado.", usuario, kindTipoOperacao.Login, kindFuncionalidade.EfetuarLogin);

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
