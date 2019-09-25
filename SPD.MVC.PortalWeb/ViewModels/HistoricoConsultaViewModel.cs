using PagedList;
using SPD.MVC.Geral.ViewModels;
using System;
using System.Collections.Generic;

namespace SPD.MVC.PortalWeb.ViewModels
{
    public class HistoricoConsultaViewModel : ViewModelBase
    {
        public int ID { get; set; }

        public int ID_Consulta { get; set; }
        public virtual ConsultaViewModel Consulta { get; set; }

        public DateTime Dt_Consulta { get; set; }

        public List<HistoricoConsultaViewModel> ListHistoricoConsultaViewModels { get; set; }

        public IPagedList<HistoricoConsultaViewModel> ListHistoricoConsulta { get; set; }

        public string DataConsulta { get; set; }
        public string Descricao { get; set; }
        public string Dentista { get; set; }
        public string Paciente { get; set; }

    }
}