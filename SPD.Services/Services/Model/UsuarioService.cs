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

        public UsuarioService(IUsuarioRepository usuarioRepository, IHistoricoOperacaoRepository historicoOperacaoRepository,
                              INotificacaoRepository notificacaoRepository)
            : base(usuarioRepository)
        {
            _UsuarioRepository = usuarioRepository;
            _HistoricoOperacaoRepository = historicoOperacaoRepository;
            _NotificacaoRepository = notificacaoRepository;
        }

        public int RedefinirSenha(string login, EmailConfiguration smtpConfiguration, string enderecoIP)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(login))
                {
                    if (smtpConfiguration.Valido())
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
                                        this._NotificacaoRepository.NotificarPorEmail(usuario.EMAIL, String.Format("Prezado(a) Usuário(a) {0}. Durante o login será solicitada uma nova senha, esta é sua senha provisória {1}", usuario.LOGIN, novaSenha), "Redefinição de Senha", smtpConfiguration);

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
                        throw new Exception(String.Format(CultureInfo.InvariantCulture, "Dados de SMTP inválidos: \"{0}\".", smtpConfiguration.ToString()));
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

    }
}
