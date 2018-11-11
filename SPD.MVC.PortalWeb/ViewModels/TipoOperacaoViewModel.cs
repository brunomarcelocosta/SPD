using SPD.MVC.Geral.ViewModels;
using SPD.MVC.PortalWeb.Content.Texts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SPD.MVC.PortalWeb.ViewModels
{
    public class TipoOperacaoViewModel : ViewModelBase
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessageResourceType = typeof(TipoOperacaoResource), ErrorMessageResourceName = "CodigoTipoOperacaoRequiredMessage")]
        [MaxLength(15, ErrorMessageResourceType = typeof(TipoOperacaoResource), ErrorMessageResourceName = "CodigoTipoOperacaoMaxLengthMessage")]
        [Display(ResourceType = typeof(TipoOperacaoResource), Name = "CodigoTipoOperacao")]
        public string Codigo_Tipo_Operacao { get; set; }

        [MaxLength(255, ErrorMessageResourceType = typeof(TipoOperacaoResource), ErrorMessageResourceName = "DescricaoTipoOperacaoMaxLengthMessage")]
        [Display(ResourceType = typeof(TipoOperacaoResource), Name = "DescricaoTipoOperacao")]
        public string Descricao_Tipo_Operacao { get; set; }
    }
}