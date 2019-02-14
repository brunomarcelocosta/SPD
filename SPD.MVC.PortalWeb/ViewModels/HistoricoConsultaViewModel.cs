using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPD.MVC.PortalWeb.ViewModels
{
    public class HistoricoConsultaViewModel
    {
        public int ID { get; set; }

        public int ID_Consulta { get; set; }
        public virtual ConsultaViewModel Consulta { get; set; }

        public DateTime Dt_Consulta { get; set; }
    }
}