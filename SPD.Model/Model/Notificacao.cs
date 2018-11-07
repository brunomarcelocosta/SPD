using SPD.Model.Util;
using System;

namespace SPD.Model.Model
{
    public class Notificacao
    {
        public int ID { get; set; } // Alterado para public devido ao mapeamento objeto-relacional do EntityFramework
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public bool Situacao { get; set; }
        public virtual Usuario Usuario { get; set; } // UML - (0..*) Notificacao é associado com (0..1) Usuario. Virtual para lazy load
        public int UsuarioID { get; set; } // Adicionado devido ao mapeamento objeto-relacional do EntityFramework

        public void NotificarPorEmail(string email, string valor, string assunto, EmailConfiguration smtpConfiguration)
        {
            smtpConfiguration.PrepararClienteSmtp().Send(smtpConfiguration.PrepararEmail(null, null, email, null, null, assunto, valor, smtpConfiguration));
        }
    }
}
