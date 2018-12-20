using SPD.Model.Model;
using SPD.Model.Util;
using SPD.Repository.Interface.Model;
using System;
using System.Net.Mail;

namespace SPD.Repository.Repository.Model
{
    public class NotificacaoRepository : RepositoryBase<Notificacao>, INotificacaoRepository
    {
        public void NotificarPorEmail(string email, string valor, string assunto, string emailFrom, string pwdFrom)
        {
            try
            {


                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.office365.com");
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(email);
                mail.Subject = assunto;
                mail.Body = valor;

                SmtpServer.Credentials = new System.Net.NetworkCredential(emailFrom, pwdFrom);
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
