using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPD.MVC.PortalWeb.ViewModels
{
    public class DentistaViewModel
    {
        public int ID { get; set; }

        public string Nome { get; set; }

        public string Cro { get; set; }

        public int ID_Usuario { get; set; }
        public virtual UsuarioViewModel Usuario { get; set; }

        public DateTime Dt_Insert { get; set; }
    }
}