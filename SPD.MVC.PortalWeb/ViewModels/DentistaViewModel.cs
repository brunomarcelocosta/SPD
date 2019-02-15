using SPD.MVC.Geral.ViewModels;
using System;
using System.Collections.Generic;

namespace SPD.MVC.PortalWeb.ViewModels
{
    public class DentistaViewModel : ViewModelBase
    {
        public int ID { get; set; }

        public string Nome { get; set; }

        public string Cro { get; set; }

        public int ID_Usuario { get; set; }
        public virtual UsuarioViewModel Usuario { get; set; }

        public DateTime Dt_Insert { get; set; }

        public List<DentistaViewModel> ListDentistaViewModel { get; set; }

        public string Usuario_string { get; set; }

    }
}