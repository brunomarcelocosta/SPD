using SPD.MVC.Geral.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPD.MVC.PortalWeb.ViewModels
{
    public class PreConsultaViewModel : ViewModelBase
    {
        public int ID { get; set; }

        public int ID_Agenda { get; set; }
        public virtual AgendaViewModel Agenda { get; set; }

        public bool Maior_Idade { get; set; }

        public bool? Autorizado { get; set; }

        public string Convenio { get; set; }

        public string Numero_Carterinha { get; set; }

        public int? ID_Assinatura { get; set; }
        public AssinaturaViewModel Assinatura { get; set; }

        public string Tipo_Pagamento { get; set; }

        public DateTime Dt_Insert { get; set; }

        public List<PreConsultaViewModel> ListPreConsultaViewModel { get; set; }
        public string Paciente_string { get; set; }
        public string Autorizado_string { get; set; }
        public bool Conveniado { get; set; }
        public string Idade { get; set; }
        public bool particular { get; set; }
        public string Nome_Responsavel { get; set; }
        public string Cpf_Responsavel { get; set; }

        public string Img_string { get; set; }

        public string Dentista { get; set; }
        public string Horario { get; set; }
    }
}