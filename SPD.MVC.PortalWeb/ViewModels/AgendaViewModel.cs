using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPD.MVC.PortalWeb.ViewModels
{
    public class AgendaViewModel
    {
        public int ID { get; set; }

        public int ID_DENTISTA { get; set; }
        public virtual DentistaViewModel DENTISTA { get; set; }

        public int? ID_PACIENTE { get; set; }
        public virtual PacienteViewModel Paciente { get; set; }

        public string NOME_PACIENTE { get; set; }

        public DateTime Data_Consulta { get; set; }

        public int ID_USUARIO { get; set; }
        public virtual UsuarioViewModel Usuario { get; set; }

        public DateTime DT_INSERT { get; set; }
    }
}