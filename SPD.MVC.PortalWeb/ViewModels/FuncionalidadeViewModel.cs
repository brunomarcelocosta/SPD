using SPD.MVC.Geral.ViewModels;
using SPD.MVC.PortalWeb.Content.Texts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SPD.MVC.PortalWeb.ViewModels
{
    public class FuncionalidadeViewModel : ViewModelBase
    {
        [Key]
        public int ID { get; set; } // Alterado para public devido ao mapeamento objeto-relacional do EntityFramework

        [Required(ErrorMessageResourceType = typeof(FuncionalidadeResource), ErrorMessageResourceName = "NomeRequiredMessage")]
        [MaxLength(50, ErrorMessageResourceType = typeof(FuncionalidadeResource), ErrorMessageResourceName = "NomeMaxLengthMessage")]
        [Display(ResourceType = typeof(FuncionalidadeResource), Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessageResourceType = typeof(FuncionalidadeResource), ErrorMessageResourceName = "isAtivoRequiredMessage")]
        [Display(ResourceType = typeof(FuncionalidadeResource), Name = "isAtivo")]
        public bool isAtivo { get; set; }
    }
}