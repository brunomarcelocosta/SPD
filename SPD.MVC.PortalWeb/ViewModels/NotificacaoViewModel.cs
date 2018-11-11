using SPD.MVC.Geral.ViewModels;
using SPD.MVC.PortalWeb.Content.Texts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SPD.MVC.PortalWeb.ViewModels
{
    public class NotificacaoViewModel : ViewModelBase
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessageResourceType = typeof(NotificacaoResource), ErrorMessageResourceName = "DescricaoRequiredMessage")]
        [MaxLength(255, ErrorMessageResourceType = typeof(NotificacaoResource), ErrorMessageResourceName = "DescricaoMaxLengthMessage")]
        [Display(ResourceType = typeof(NotificacaoResource), Name = "Descricao")]
        public string Descricao { get; set; }

        [Required(ErrorMessageResourceType = typeof(NotificacaoResource), ErrorMessageResourceName = "DataRequiredMessage")]
        [Display(ResourceType = typeof(NotificacaoResource), Name = "Data")]
        public DateTime Data { get; set; }

        [Required(ErrorMessageResourceType = typeof(NotificacaoResource), ErrorMessageResourceName = "SituacaoRequiredMessage")]
        [Display(ResourceType = typeof(NotificacaoResource), Name = "Situacao")]
        public bool Situacao { get; set; }

        public int ID_Usuario { get; set; }
        public virtual UsuarioViewModel Usuario { get; set; }
    }
}