using SPD.Model.Enums;
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
    public class UsuarioService : ServiceBase<Usuario>, IUsuarioService
    {
        private readonly IUsuarioRepository _UsuarioRepository;
        private readonly IHistoricoOperacaoRepository _HistoricoOperacaoRepository;
        private readonly INotificacaoRepository _NotificacaoRepository;
        private readonly ISessaoUsuarioService _SessaoUsuarioService;
        private readonly IUsuarioFuncionalidadeRepository _UsuarioFuncionalidadeRepository;
        private readonly IFuncionalidadeRepository _FuncionalidadeRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository, IHistoricoOperacaoRepository historicoOperacaoRepository,
                              INotificacaoRepository notificacaoRepository, ISessaoUsuarioService sessaoUsuarioService,
                              IUsuarioFuncionalidadeRepository usuarioFuncionalidadeRepository, IFuncionalidadeRepository funcionalidadeRepository)
            : base(usuarioRepository)
        {
            _UsuarioRepository = usuarioRepository;
            _HistoricoOperacaoRepository = historicoOperacaoRepository;
            _NotificacaoRepository = notificacaoRepository;
            _SessaoUsuarioService = sessaoUsuarioService;
            _UsuarioFuncionalidadeRepository = usuarioFuncionalidadeRepository;
            _FuncionalidadeRepository = funcionalidadeRepository;
        }

        public int RedefinirSenha(string login, string emailFrom, string pwdFrom, string enderecoIP)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(login))
                {

                    var usuario = this._UsuarioRepository.GetByLogin(login);

                    if (usuario != null)
                    {
                        string novaSenha;

                        if (RedefinirSenha(usuario, out novaSenha))
                        {
                            this._HistoricoOperacaoRepository.RegistraHistorico(String.Format(CultureInfo.InvariantCulture, "Senha do usuário {0} redefinida.", usuario.LOGIN), usuario, Tipo_Operacao.Senha, Tipo_Funcionalidades.Usuarios);

                            try
                            {
                                using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                                {
                                    this._NotificacaoRepository.NotificarPorEmail(usuario.EMAIL, String.Format("Prezado(a) Usuário(a) {0}. Durante o login será solicitada uma nova senha, esta é sua senha provisória {1}", usuario.LOGIN, novaSenha), "Redefinição de Senha", emailFrom, pwdFrom);

                                    this.SaveChanges(transactionScope);
                                }
                            }
                            catch (SmtpException Exception)
                            {
                                return 2;
                            }
                            finally
                            {
                                this.SaveChanges();
                            }

                            return 1;
                        }
                        else
                        {
                            //using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                            //{
                            this._HistoricoOperacaoRepository.RegistraHistorico(String.Format(CultureInfo.InvariantCulture, "Não foi possível alterar a senha do usuário: \"{0}\".", usuario.LOGIN), usuario, Tipo_Operacao.Senha, Tipo_Funcionalidades.Usuarios);

                            this.SaveChanges();
                            //}
                        }
                    }
                    else
                    {
                        throw new Exception(String.Format(CultureInfo.InvariantCulture, "O usuário \"{0}\" não está cadastrado no banco de dados do sistema.", login));
                    }
                }
                else
                {
                    throw new Exception(String.Format(CultureInfo.InvariantCulture, "Login inválido."));
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }

            return 0;
        }

        public bool RedefinirSenha(Usuario usuario, out string novaSenha)
        {
            if (usuario.RedefinirSenha(out novaSenha))
            {
                this._UsuarioRepository.Atualizar(usuario);

                return true;
            }

            return false;
        }

        public bool ConfirmarSenha(int ID, string password, out string result)
        {
            string mensagemHistorico = null;

            var usuario = _UsuarioRepository.GetById(ID);

            try
            {
                using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                {
                    usuario.PASSWORD = Usuario.GerarHash(password);

                    if (usuario.TROCA_SENHA_OBRIGATORIA)
                    {
                        mensagemHistorico = "Alteração obrigatória da senha do usuário";
                    }
                    else
                    {
                        mensagemHistorico = "Alteração da senha do usuário";
                    }

                    usuario.TROCA_SENHA_OBRIGATORIA = false;
                    _UsuarioRepository.Atualizar(usuario);

                    _HistoricoOperacaoRepository.RegistraHistorico(mensagemHistorico, usuario, Tipo_Operacao.Senha, Tipo_Funcionalidades.Usuarios);

                    SaveChanges(transactionScope);
                }
            }
            catch (Exception ex)
            {
                result = "erro ao salvar: " + ex.Message;
                return false;
            }

            result = "Senha alterada com sucesso.";
            return true;
        }

        public bool Desbloquear(Usuario usuarioDesbloqueio, int idUsuarioAtual)
        {
            try
            {
                using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                {
                    Usuario usuarioAtual = this._UsuarioRepository.GetById(idUsuarioAtual);

                    if (this._UsuarioRepository.DesBloquear(usuarioDesbloqueio)) //executa o desbloqueio
                    {
                        //Registra desbloqueio no histórico de operações
                        this._HistoricoOperacaoRepository.RegistraHistorico(String.Format(CultureInfo.InvariantCulture, "Desbloqueou o usuário: \"{0}\".", usuarioDesbloqueio.LOGIN), usuarioAtual, Tipo_Operacao.Alteracao, Tipo_Funcionalidades.Usuarios);
                    }
                    this.SaveChanges(transactionScope);
                }

            }
            catch
            {
                return false;
            }
            finally
            {
                this.SaveChanges();

            }
            return true;
        }

        public bool Desconectar(Usuario usuario)
        {
            try
            {
                using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                {
                    this._SessaoUsuarioService.EncerrarSessao(usuario.ID, String.Format("Usuário {0} desconectado do sistema.", usuario.LOGIN));
                    this.SaveChanges(transactionScope);
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                this.SaveChanges();

            }
            return true;
        }

        public bool UpdateUser(Usuario usuario, Usuario usuario_logado, List<UsuarioFuncionalidade> usuarioFuncionalidades_ADD, List<UsuarioFuncionalidade> usuarioFuncionalidades_DEL, out string resultado)
        {
            resultado = "";

            try
            {
                using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                {
                    // atualizar usuario
                    _UsuarioRepository.AtualizarUsuario(usuario);

                    if (_HistoricoOperacaoRepository.RegistraHistoricoRepository(String.Format(CultureInfo.InvariantCulture, "Atualizou o registro {0} referente à Usuário", usuario.LOGIN), usuario_logado, Tipo_Operacao.Alteracao, Tipo_Funcionalidades.Usuarios, out resultado))
                    {
                        return false;
                    }
                    // add usuario_funcionalidade
                    if (!_UsuarioFuncionalidadeRepository.AddList(usuarioFuncionalidades_ADD, out resultado))
                    {
                        return false;
                    }

                    // del usuario_funcionalidade
                    if (!_UsuarioFuncionalidadeRepository.DeleteList(usuarioFuncionalidades_DEL, out resultado))
                    {
                        return false;
                    }

                    SaveChanges(transactionScope);
                }
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
                return false;
            }

            return true;
        }

        public bool AddNewUser(Usuario usuario, Usuario usuario_logado, List<UsuarioFuncionalidade> usuarioFuncionalidades_ADD, string emailFrom, string pwdFrom, out string resultado)
        {
            resultado = "";

            if (usuarioFuncionalidades_ADD.Count() == 0 || usuarioFuncionalidades_ADD == null)
            {
                resultado = "Funcionalidade deve ser selecionada.";
                return false;
            }

            Usuario usuarioBD = this._UsuarioRepository.Query().Where(a => a.LOGIN == usuario.LOGIN).FirstOrDefault();
            if (usuarioBD != null)
            {
                resultado = String.Format(CultureInfo.InvariantCulture, "O login informado já existe.", usuario.LOGIN);
                return false;
            }

            string senha = usuario.GenerateNewPassword(8, true, true, true, false).ToString();
            usuario.PASSWORD = Usuario.GerarHash(senha);
            usuario.TROCA_SENHA_OBRIGATORIA = true;
            usuario.IsBLOQUEADO = false;
            usuario.TENTATIVAS_LOGIN = 0;

            try
            {
                using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                {
                    this._NotificacaoRepository.NotificarPorEmail(usuario.EMAIL, String.Format("Prezado(a) Usuário(a) esta é sua senha provisória {0} . Durante o login, será solicitada uma nova senha.", senha), "Senha de acesso ao sistema SPD", emailFrom, pwdFrom);
                    this.SaveChanges(transactionScope);
                }
            }
            catch
            {
                resultado = "Caixa de entrada do domínio de email " + usuario.EMAIL + " indisponível.";
                return false;
            }
            finally
            {
                this.SaveChanges();
            }

            try
            {
                using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                {
                    this._HistoricoOperacaoRepository.RegistraHistorico(String.Format(CultureInfo.InvariantCulture, "Senha gerada com sucesso, e enviada ao e-mail do usuário {0}.", usuario.LOGIN.ToUpper()), usuario_logado, Tipo_Operacao.Inclusao, Tipo_Funcionalidades.Usuarios);

                    // atualizar usuario
                    _UsuarioRepository.Add(usuario);

                    _HistoricoOperacaoRepository.RegistraHistorico(String.Format(CultureInfo.InvariantCulture, "Adicionou o registro {0} referente à Usuário", usuario.LOGIN), usuario_logado, Tipo_Operacao.Inclusao, Tipo_Funcionalidades.Usuarios);

                    var user = _UsuarioRepository.Query().Where(a => a.LOGIN == usuario.LOGIN).FirstOrDefault();

                    if (user != null || user.ID != 0)
                    {
                        // add usuario_funcionalidade
                        if (!_UsuarioFuncionalidadeRepository.AddNewList(usuarioFuncionalidades_ADD, user, out resultado))
                        {
                            return false;
                        }
                        else
                        {
                            foreach (var item in usuarioFuncionalidades_ADD)
                            {
                                _HistoricoOperacaoRepository.RegistraHistorico(String.Format(CultureInfo.InvariantCulture, "Funcionalidade {0} associadas ao usuário {1}.", _FuncionalidadeRepository.GetById(item.ID_FUNCIONALIDADE).NOME, usuario.LOGIN), usuario_logado, Tipo_Operacao.Inclusao, Tipo_Funcionalidades.Usuarios);
                            }
                        }
                    }
                    else
                    {
                        resultado = "Usuario não localizado.";
                        return false;
                    }

                    SaveChanges(transactionScope);
                }
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
                return false;
            }
            finally
            {
                SaveChanges();
            }

            return true;
        }

        public bool DeleteUser(int id, Usuario usuario_logado, out string resultado)
        {
            resultado = "";

            try
            {
                var usuarioDelete = _UsuarioRepository.GetById(id);
                var usuarioFuncsDelete = _UsuarioFuncionalidadeRepository.Query().Where(a => a.ID_USUARIO == id).ToList();
                var sessao = _SessaoUsuarioService.GetSessaoByUsuarioID(id);

                using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                {
                    _HistoricoOperacaoRepository.ExcluiHistoricoUsuario(usuarioDelete);

                    _SessaoUsuarioService.EncerrarSessao(sessao);

                    if (!_UsuarioFuncionalidadeRepository.DeleteList(usuarioFuncsDelete, out resultado))
                    {
                        return false;
                    }

                    _HistoricoOperacaoRepository.RegistraHistorico(String.Format("Usuário {0} excluído.", usuarioDelete.LOGIN), usuario_logado, Tipo_Operacao.Exclusao, Tipo_Funcionalidades.Usuarios);

                    _UsuarioRepository.Remove(usuarioDelete);

                    SaveChanges(transactionScope);
                }
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
                return false;
            }
            finally
            {
                SaveChanges();
            }

            return true;
        }
    }
}
