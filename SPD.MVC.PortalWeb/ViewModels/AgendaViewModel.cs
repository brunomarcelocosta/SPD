using SPD.MVC.Geral.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPD.MVC.PortalWeb.ViewModels
{
    public class AgendaViewModel : ViewModelBase
    {
        public int ID { get; set; }

        public int ID_Dentista { get; set; }
        public virtual DentistaViewModel Dentista { get; set; }

        public int? ID_Paciente { get; set; }
        public virtual PacienteViewModel Paciente { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Nome_Paciente { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Celular { get; set; }

        public string Data_Consulta { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Hora_Inicio { get; set; }

        public string Hora_Fim { get; set; }

        public int ID_Usuario { get; set; }
        public virtual UsuarioViewModel Usuario { get; set; }

        public DateTime Dt_Insert { get; set; }

        public List<AgendaViewModel> ListAgendaViewModel { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Dentista_string { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Tempo_Consulta { get; set; }

    }
}