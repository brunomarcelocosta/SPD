using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Net.Mail;

namespace SPD.Model.Util
{
    public sealed class EmailConfiguration
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public bool SSL { get; set; }

        public bool Valido()
        {
            return (!String.IsNullOrWhiteSpace(this.UserName) &&
                !String.IsNullOrWhiteSpace(this.UserEmail) &&
                !String.IsNullOrWhiteSpace(this.UserPassword) &&
                !String.IsNullOrWhiteSpace(this.Server) &&
                this.Port > 0);
        }
        
        public SmtpClient PrepararClienteSmtp(bool useDefaultCredentials = false)
        {
            var smtpClient = new SmtpClient(this.Server);

            smtpClient.UseDefaultCredentials = useDefaultCredentials;
            smtpClient.Credentials = new NetworkCredential(this.UserEmail, this.UserPassword);
            smtpClient.Port = this.Port;
            smtpClient.EnableSsl = this.SSL;

            return smtpClient;
        }

        public MailMessage PrepararEmail(string from, string fromName, string to, string cc, string bcc, string subject, string body, EmailConfiguration smtpConfiguration)
        {
            var mailMessage = new MailMessage();

            if (!String.IsNullOrWhiteSpace(from) && !String.IsNullOrWhiteSpace(fromName))
            {
                mailMessage.From = new MailAddress(from, fromName);
            }
            else
            {
                mailMessage.From = new MailAddress(smtpConfiguration.UserEmail, smtpConfiguration.UserName);
            }

            mailMessage.To.Add(new MailAddress(to));

            if (!String.IsNullOrWhiteSpace(cc))
            {
                mailMessage.CC.Add(cc);
            }

            if (!String.IsNullOrWhiteSpace(bcc))
            {
                mailMessage.Bcc.Add(bcc);
            }

            body = BuildWithTemplate(body, subject);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            return mailMessage;
        }
        
        private string BuildWithTemplate(string body, string subject)
        {
            string template = "<!DOCTYPE html PUBLIC \" -//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html><head> <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\"> <title>SGTAN - EMAIL</title></head> <body offset=\"0\" topmargin=\"0\" marginwidth=\"0\" marginheight=\"0\" leftmargin=\"0\"><div style=\"width=100%; font-family:arial\"> <div style=\"width: 50%;margin: 0 auto; background-color:#FBFBFB; padding:8px; box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19);\"> <table class=\"table table-striped table-bordered\"> <thead> <tr> <th style=\"background-color:#dae2e3;\" width=\"150px\" height=\"90px\"> <h3>SGTAN</h3> </th> <th style=\"text-align:center;\" width=\"600px\" height=\"90px\"> <center style=\"margin-top:20px\"> <h4>==TITULO=DA=LISTAGEM==</h4> </center> </th> </tr></thead> </table> <div style=\"margin:10px; font-size:0.8em\"> <p> Prezado(a),</p><p>==CORPO=EMAIL==</p></div></div></div></body></html>";
            template = template.Replace("==TITULO=DA=LISTAGEM==", subject);
            template = template.Replace("==CORPO=EMAIL==", body);

            return template;
        }
        
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "UserName: \"{0}\" - UserEmail: \"{1}\" - Password: \"{2}\" - Server: \"{3}\" - Port: \"{4}\" - SSL: \"{5}\"", this.UserName, this.UserEmail, this.UserPassword, this.Server, this.Port, this.SSL);
        }
        
        public static EmailConfiguration FromEmailSettings()
        {
            var emailConfiguration = new EmailConfiguration();

            var section = (NameValueCollection)ConfigurationManager.GetSection("emailSettings");

            emailConfiguration.UserName = section["userName"];
            emailConfiguration.UserEmail = section["userEmail"];
            emailConfiguration.UserPassword = section["userPassword"];
            emailConfiguration.Server = section["server"];
            emailConfiguration.Port = Int32.Parse(section["port"]);
            emailConfiguration.SSL = Boolean.Parse(section["ssl"]);

            return emailConfiguration;
        }
    }
}
