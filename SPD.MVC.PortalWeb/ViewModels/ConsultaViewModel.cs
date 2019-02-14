using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPD.MVC.PortalWeb.ViewModels
{
    public class ConsultaViewModel
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
    }
}