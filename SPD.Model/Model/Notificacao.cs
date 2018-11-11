using SPD.Model.Util;
using System;

namespace SPD.Model.Model
{
    public class Notificacao
    {
        public int ID { get; set; }

        public string DESCRICAO { get; set; }

        public DateTime DATA { get; set; }

        public bool SITUACAO { get; set; }

        public int ID_USUARIO { get; set; }
        public virtual Usuario usuario { get; set; }

        public void NotificarPorEmail(string email, string valor, string assunto, EmailConfiguration smtpConfiguration)
        {
            smtpConfiguration.PrepararClienteSmtp().Send(smtpConfiguration.PrepararEmail(null, null, email, null, null, assunto, valor, smtpConfiguration));
        }
    }
}
