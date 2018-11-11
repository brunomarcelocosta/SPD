using SPD.MVC.Geral.ViewModels;
using SPD.MVC.PortalWeb.Content.Texts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace SPD.MVC.PortalWeb.ViewModels
{
    public class SessaoUsuarioViewModel : ViewModelBase
    {
        [Key]
        public int ID { get; set; } 

        [Required(ErrorMessageResourceType = typeof(SessaoUsuarioResource), ErrorMessageResourceName = "DataHoraAcessoRequiredMessage")]
        [Display(ResourceType = typeof(SessaoUsuarioResource), Name = "DataHoraAcesso")]
        public DateTime DataHoraAcesso { get; set; }

        [Required(ErrorMessageResourceType = typeof(SessaoUsuarioResource), ErrorMessageResourceName = "EnderecoIPRequiredMessage")]
        [MaxLength(15, ErrorMessageResourceType = typeof(SessaoUsuarioResource), ErrorMessageResourceName = "EnderecoIPMaxLengthMessage")]
        [Display(ResourceType = typeof(SessaoUsuarioResource), Name = "EnderecoIP")]
        public string EnderecoIP { get; set; }

        public int ID_Usuario { get; set; } 
        public virtual UsuarioViewModel Usuario { get; set; } 
    }
}