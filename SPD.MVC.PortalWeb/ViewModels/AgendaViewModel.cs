using SPD.MVC.Geral.ViewModels;
using System;
using System.Collections.Generic;

namespace SPD.MVC.PortalWeb.ViewModels
{
    public class AgendaViewModel : ViewModelBase
    {
        public int ID { get; set; }

        public int ID_Dentista { get; set; }
        public virtual DentistaViewModel Dentista { get; set; }

        public int? ID_Paciente { get; set; }
        public virtual PacienteViewModel Paciente { get; set; }

        public string Nome_Paciente { get; set; }

        public string Data_Consulta { get; set; }

        public string Hora_Inicio { get; set; }

        public string Hora_Fim { get; set; }

        public int ID_Usuario { get; set; }
        public virtual UsuarioViewModel Usuario { get; set; }

        public DateTime Dt_Insert { get; set; }

        public List<AgendaViewModel> ListAgendaViewModel { get; set; }
        public string Dentista_string { get; set; }
        public string Hora_string { get; set; }
        public string Tempo_Consulta { get; set; }

    }
}