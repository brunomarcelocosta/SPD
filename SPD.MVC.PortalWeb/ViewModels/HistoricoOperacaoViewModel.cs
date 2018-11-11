using SPD.MVC.Geral.ViewModels;
using SPD.MVC.PortalWeb.Content.Texts;
using System;
using System.ComponentModel.DataAnnotations;
using static SPD.Model.Enums.SPD_Enums;

namespace SPD.MVC.PortalWeb.ViewModels
{
    public class HistoricoOperacaoViewModel : ViewModelBase
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(15, ErrorMessageResourceType = typeof(HistoricoOperacaoResource), ErrorMessageResourceName = "EnderecoIPMaxLengthMessage")]
        [Display(ResourceType = typeof(HistoricoOperacaoResource), Name = "EnderecoIP")]
        public string EnderecoIP { get; set; }

        [Required(ErrorMessageResourceType = typeof(HistoricoOperacaoResource), ErrorMessageResourceName = "DescricaoRequiredMessage")]
        [MaxLength(4000, ErrorMessageResourceType = typeof(HistoricoOperacaoResource), ErrorMessageResourceName = "DescricaoMaxLengthMessage")]
        [Display(ResourceType = typeof(HistoricoOperacaoResource), Name = "Descricao")]
        public string Descricao { get; set; }

        [Display(ResourceType = typeof(HistoricoOperacaoResource), Name = "KindTipoOperacao")]
        public Tipo_Operacao KindTipoOperacao { get; set; }

        [Required(ErrorMessageResourceType = typeof(HistoricoOperacaoResource), ErrorMessageResourceName = "DataOperacaoRequiredMessage")]
        [Display(ResourceType = typeof(HistoricoOperacaoResource), Name = "DataOperacao")]
        public DateTime DataOperacao { get; set; }

        [MaxLength(4000, ErrorMessageResourceType = typeof(HistoricoOperacaoResource), ErrorMessageResourceName = "DumpMaxLengthMessage")]
        [Display(ResourceType = typeof(HistoricoOperacaoResource), Name = "Dump")]
        public string Dump { get; set; }

        public int ID_Usuario { get; set; }
        public virtual UsuarioViewModel Usuario { get; set; }


        public int ID_Tipo_Operacao { get; set; } // Adicionado devido ao mapeamento objeto-relacional do EntityFramework
        public virtual TipoOperacaoViewModel TipoOperacao { get; set; } // Adicionado devido ao mapeamento objeto-relacional do EntityFramework. Virtual para lazy load

        [NonSerialized]
        public int? ID_Funcionalidade;
        public virtual FuncionalidadeViewModel Funcionalidade { get; set; } // UML - (0..*) HistoricoOperacao é agregado com (1) Funcionalidade. Virtual para lazy load


        [Display(ResourceType = typeof(HistoricoOperacaoResource), Name = "SessaoUID")]
        private string SessaoUID { get; set; }


    }
}