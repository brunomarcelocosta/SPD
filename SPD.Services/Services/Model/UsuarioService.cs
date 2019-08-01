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

                        if (RedefinirSenha(usuario, out string novaSenha))
                        {
                            this._HistoricoOperacaoRepository.Insert($"Senha do usuário {usuario.LOGIN} redefinida.", usuario, Tipo_Operacao.Senha, Tipo_Funcionalidades.Usuarios);

                            try
                            {
                                //using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                                //{
                                this._NotificacaoRepository.NotificarPorEmail(usuario.EMAIL, String.Format("Prezado(a) Usuário(a) {0}. Durante o login será solicitada uma nova senha, esta é sua senha provisória {1}", usuario.LOGIN, novaSenha), "Redefinição de Senha", emailFrom, pwdFrom);

                                //this.SaveChanges(transactionScope);
                                //}
                            }
                            catch (SmtpException)
                            {
                                return 2;
                            }


                            return 1;
                        }

                        else
                        {
                            //using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                            //{
                            this._HistoricoOperacaoRepository.Insert($"Não foi possível alterar a senha do usuário: \"{usuario.LOGIN}\".", usuario, Tipo_Operacao.Senha, Tipo_Funcionalidades.Usuarios);

                            //this.SaveChanges();
                            //}
                        }
                    }
                    else
                    {
                        throw new Exception($"O usuário \"{login}\" não está cadastrado no banco de dados do sistema.");
                    }
                }
                else
                {
                    throw new Exception($"Login inválido.");
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
                //using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                //{
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

                _HistoricoOperacaoRepository.Insert(mensagemHistorico, usuario, Tipo_Operacao.Senha, Tipo_Funcionalidades.Usuarios);

                //  SaveChanges(transactionScope);
                //}
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
                //using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                //{
                Usuario usuarioAtual = this._UsuarioRepository.GetById(idUsuarioAtual);

                if (this._UsuarioRepository.DesBloquear(usuarioDesbloqueio)) //executa o desbloqueio
                {
                    //Registra desbloqueio no histórico de operações
                    this._HistoricoOperacaoRepository.Insert($"Desbloqueou o usuário: \"{usuarioDesbloqueio.LOGIN}\".", usuarioAtual, Tipo_Operacao.Alteracao, Tipo_Funcionalidades.Usuarios);
                }
                // this.SaveChanges(transactionScope);
                // }

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
                // using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                //{
                this._SessaoUsuarioService.EncerrarSessao(usuario.ID, $"Usuário {usuario.LOGIN} desconectado do sistema.");
                //this.SaveChanges(transactionScope);
                // }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool UpdateUser(Usuario usuario, Usuario usuario_logado, List<UsuarioFuncionalidade> usuarioFuncionalidades_ADD, List<UsuarioFuncionalidade> usuarioFuncionalidades_DEL, out string resultado)
        {
            resultado = "";

            try
            {
                //using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                //{
                // atualizar usuario
                _UsuarioRepository.AtualizarUsuario(usuario);

                _HistoricoOperacaoRepository.Insert($"Atualizou o registro {usuario.LOGIN} referente à Usuário", usuario_logado, Tipo_Operacao.Alteracao, Tipo_Funcionalidades.Usuarios);

                //   SaveChanges(transactionScope);
                // }
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
                return false;
            }
            finally
            {
                this.SaveChanges();
            }

            if (usuarioFuncionalidades_ADD.Count > 0)
            {
                if (!AddUserAndFuncs(usuarioFuncionalidades_ADD, usuario, usuario_logado, out resultado))
                {
                    return false;
                }
            }

            if (usuarioFuncionalidades_DEL.Count > 0)
            {
                if (!DELUserAndFuncs(usuarioFuncionalidades_DEL, usuario, usuario_logado, out resultado))
                {
                    return false;
                }
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
                //using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                //{
                _NotificacaoRepository.NotificarPorEmail(usuario.EMAIL, $"Prezado(a) Usuário(a) esta é sua senha provisória {senha} . Durante o login, será solicitada uma nova senha.", "Senha de acesso ao sistema SPD", emailFrom, pwdFrom);

                _HistoricoOperacaoRepository.Insert($"Senha gerada com sucesso, e enviada ao e-mail do usuário {usuario.LOGIN.ToUpper()}.", usuario_logado, Tipo_Operacao.Inclusao, Tipo_Funcionalidades.Usuarios);

                _UsuarioRepository.Insert(usuario);

                _HistoricoOperacaoRepository.Insert($"Adicionou o registro {usuario.LOGIN} referente à Usuário", usuario_logado, Tipo_Operacao.Inclusao, Tipo_Funcionalidades.Usuarios);

                // SaveChanges(transactionScope);
                // }
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
                return false;
            }

            var user = _UsuarioRepository.Query().Where(a => a.LOGIN == usuario.LOGIN).FirstOrDefault();

            if (!AddUserAndFuncs(usuarioFuncionalidades_ADD, user, usuario_logado, out resultado))
            {
                return false;
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

                //using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                //{

                _HistoricoOperacaoRepository.Delete(usuarioDelete);

                if (sessao != null)
                    _SessaoUsuarioService.EncerrarSessao(sessao);

                //    SaveChanges(transactionScope);
                //}

                if (!DELUserAndFuncs(usuarioFuncsDelete, usuarioDelete, usuario_logado, out resultado))
                {
                    return false;
                }

                //using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                //{
                var login = usuarioDelete.LOGIN;

                _UsuarioRepository.Delete(usuarioDelete);

                _HistoricoOperacaoRepository.Insert($"Usuário {login} excluído.", usuario_logado, Tipo_Operacao.Exclusao, Tipo_Funcionalidades.Usuarios);

                // SaveChanges(transactionScope);
                //}
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
                return false;
            }

            return true;
        }

        public List<UsuarioFuncionalidade> HashEntityForUserAndFuncs(List<UsuarioFuncionalidade> usuarioFuncionalidades_ADD, Usuario usuario)
        {
            List<UsuarioFuncionalidade> list = new List<UsuarioFuncionalidade>();

            foreach (var item in usuarioFuncionalidades_ADD)
            {
                var func = _FuncionalidadeRepository.Query().Where(a => a.ID == item.Funcionalidade.ID).FirstOrDefault();

                item.Usuario = usuario;
                item.Funcionalidade = func;

                list.Add(item);
            }

            return list;
        }

        public bool AddUserAndFuncs(List<UsuarioFuncionalidade> usuarioFuncionalidades_ADD, Usuario usuario, Usuario usuario_logado, out string resultado)
        {
            resultado = "";

            try
            {
                var user = _UsuarioRepository.GetById(usuario.ID);

                //using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                //{
                usuarioFuncionalidades_ADD = HashEntityForUserAndFuncs(usuarioFuncionalidades_ADD, user);
                //usuarioFuncionalidades_DEL = HashEntityForUserAndFuncs(usuarioFuncionalidades_DEL, user);

                // add usuario_funcionalidade
                if (!_UsuarioFuncionalidadeRepository.AddList(usuarioFuncionalidades_ADD, out resultado))
                {
                    return false;
                }
                else
                {
                    foreach (var item in usuarioFuncionalidades_ADD)
                    {
                        _HistoricoOperacaoRepository.Insert(String.Format(CultureInfo.InvariantCulture, "Funcionalidade {0} associadas ao usuário {1}.", _FuncionalidadeRepository.GetById(item.ID_FUNCIONALIDADE).NOME, usuario.LOGIN), usuario_logado, Tipo_Operacao.Inclusao, Tipo_Funcionalidades.Usuarios);
                    }
                }

                //    SaveChanges(transactionScope);
                //}
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
                return false;
            }
            finally
            {
                this.SaveChanges();
            }

            return true;
        }

        public bool DELUserAndFuncs(List<UsuarioFuncionalidade> usuarioFuncionalidades_DEL, Usuario usuario, Usuario usuario_logado, out string resultado)
        {
            resultado = "";

            try
            {
                var user = _UsuarioRepository.GetById(usuario.ID);

                // using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                // {
                //  usuarioFuncionalidades_DEL = HashEntityForUserAndFuncs(usuarioFuncionalidades_DEL, user);

                //del usuario_funcionalidade
                if (!_UsuarioFuncionalidadeRepository.DeleteList(usuarioFuncionalidades_DEL, out resultado))
                {
                    return false;
                }
                else
                {
                    foreach (var item in usuarioFuncionalidades_DEL)
                    {
                        var func = _FuncionalidadeRepository.GetById(item.ID_FUNCIONALIDADE).NOME;
                        _HistoricoOperacaoRepository.Insert(String.Format(CultureInfo.InvariantCulture, "Funcionalidade {0} excluída do usuário {1}.", func, usuario.LOGIN), usuario_logado, Tipo_Operacao.Exclusao, Tipo_Funcionalidades.Usuarios);
                    }
                }

                //SaveChanges(transactionScope);
                //}
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
                return false;
            }


            return true;
        }

    }
}
