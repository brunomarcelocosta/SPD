using SPD.MVC.Geral.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPD.MVC.PortalWeb.ViewModels
{
    public class UsuarioFuncionalidadeViewModel : ViewModelBase
    {
        public int ID { get; set; }

        public int ID_Usuario { get; set; }
        public virtual UsuarioViewModel Usuario { get; set; }

        public int ID_FUNCIONALIDADE { get; set; }
        public virtual FuncionalidadeViewModel Funcionalidade { get; set; }
    }
}