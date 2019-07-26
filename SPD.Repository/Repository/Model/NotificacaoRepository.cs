using SPD.Model.Model;
using SPD.Model.Util;
using SPD.Repository.Interface.Model;
using System;
using System.Net;
using System.Net.Mail;

namespace SPD.Repository.Repository.Model
{
    public class NotificacaoRepository : RepositoryBase<Notificacao>, INotificacaoRepository
    {
        public void NotificarPorEmail(string email, string valor, string assunto, string emailFrom, string pwdFrom)
        {
            try
            {
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(emailFrom)
                };
                mail.To.Add(email);
                mail.Subject = assunto;
                mail.Body = valor;

                SmtpClient SmtpServer = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                };

                SmtpServer.Credentials = new NetworkCredential(emailFrom, pwdFrom);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //Notificacao notificacao = new Notificacao();

            //notificacao.NotificarPorEmail(email, valor, assunto, smtpConfiguration);
        }
    }
}
