using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPD.MVC.PortalWeb.ViewModels
{
    public class PreConsultaViewModel
    {
        public int ID { get; set; }

        public int ID_Paciente { get; set; }
        public virtual PacienteViewModel Paciente { get; set; }

        public bool Maior_Idade { get; set; }

        public bool? Autorizado { get; set; }

        public byte[] Autorizacao { get; set; }

        public string Convenio { get; set; }

        public string Numero_Carterinha { get; set; }

        public string Valor_Consulta { get; set; }

        public string Tipo_Pagamento { get; set; }

        public DateTime Dt_Insert{ get; set; }
    }
}