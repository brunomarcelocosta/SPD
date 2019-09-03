using SPD.MVC.Geral.ViewModels;
using System;
using System.Collections.Generic;

namespace SPD.MVC.PortalWeb.ViewModels
{
    public class ConsultaViewModel : ViewModelBase
    {
        public int ID { get; set; }

        public int ID_Dentista { get; set; }
        public virtual DentistaViewModel Dentista { get; set; }

        public int ID_Pre_Consulta { get; set; }
        public virtual PreConsultaViewModel Pre_Consulta { get; set; }

        public string Descricao_Procedimento { get; set; }

        public byte[] Odontograma { get; set; }

        public byte[] Exame { get; set; }

        public string Comentarios { get; set; }

        public DateTime Dt_Consulta { get; set; }


        public string Paciente_string { get; set; }
        public string Dentista_string { get; set; }
        public string Convenio_string { get; set; }
        public string Idade_string { get; set; }
        public string Celular { get; set; }
        public string Img_string { get; set; }

        public List<PreConsultaViewModel> ListPreConsultaViewModel { get; set; }

        public List<HistoricoConsultaViewModel> ListHistoricoConsultaViewModels { get; set; }

    }
}