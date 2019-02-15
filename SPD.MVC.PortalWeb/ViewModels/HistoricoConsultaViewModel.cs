using SPD.MVC.Geral.ViewModels;
using System;

namespace SPD.MVC.PortalWeb.ViewModels
{
    public class HistoricoConsultaViewModel : ViewModelBase
    {
        public int ID { get; set; }

        public int ID_Consulta { get; set; }
        public virtual ConsultaViewModel Consulta { get; set; }

        public DateTime Dt_Consulta { get; set; }
    }
}