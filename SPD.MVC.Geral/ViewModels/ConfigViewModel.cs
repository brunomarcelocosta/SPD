using SPD.MVC.Geral.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.MVC.Geral.ViewModels
{
    public class ConfigModuloViewModel
    {
        public string Nome { get; set; }
        public string Porta { get; set; }
    }

    public class ConfigViewModel
    {
        public string Url { get; set; }
        public List<ConfigModuloViewModel> Modulos { get; set; }
        public string Domain { get; set; } = GlobalConstants.General.Domain;

        public string GetUrl(string modulo)
        {

            var retUrl = string.Empty;
            if (string.IsNullOrEmpty(modulo))
            {
                return retUrl;
            }

            var retModulo = Modulos.FirstOrDefault(x => x.Nome == modulo);

            if (retModulo != null)
            {
                retUrl = Url + ":" + retModulo.Porta;
            }

            return retUrl;
        }

    }

}
