using SPD.MVC.Geral.ViewModels;
using SPD.MVC.PortalWeb.Content.Texts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SPD.MVC.PortalWeb.ViewModels
{
    public class UsuarioViewModel : ViewModelBase
    {
        [Key]
        public int ID { get; set; } // Alterado para public devido ao mapeamento objeto-relacional do EntityFramework

        [Required(ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "NomeRequiredMessage")]
        [MaxLength(255, ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "NomeMaxLengthMessage")]
        public string Nome { get; set; }

        [Required(ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "EmailRequiredMessage")]
        [MaxLength(50, ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "EmailMaxLengthMessage")]
        [EmailAddress(ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "EmailEmailAddressMessage", ErrorMessage = null)]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "LoginRequiredMessage")]
        [MaxLength(25, ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "LoginMaxLengthMessage")]
        [MinLength(8, ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "LoginMinLengthMessage")]
        public string Login { get; set; }

        [Required(ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "PasswordRequiredMessage")]
        [MaxLength(50, ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "PasswordMaxLengthMessage")]
        [MinLength(8, ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "PasswordMinLengthMessage")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "PasswordRule1Message")]
        [DataType(DataType.Password)]
        public string Password { get; set; } // Alterado para public devido ao mapeamento objeto-relacional do EntityFramework

        // ToDo: Adicionado para o cadastro. Talvez remover.
        [MaxLength(50, ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "PasswordMaxLengthMessage")]
        [Compare("Password", ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "PasswordConfirmacaoCompareMessage")]
        [DataType(DataType.Password)]
        public string PasswordConfirmacao { get; set; }

        //public int TentativasLogin { get; set; } // Alterado para public devido ao mapeamento objeto-relacional do EntityFramework
        public int Tentativas_Login { get; set; }

        [Required(ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "isAtivoRequiredMessage")]
        public bool isAtivo { get; set; }

        public bool isBloqueado { get; set; }

        public DateTime UltimaTrocaSenha { get; set; } // Alterado para public devido ao mapeamento objeto-relacional do EntityFramework

        public bool TrocaSenhaObrigatoria { get; set; } // Alterado para public devido ao mapeamento objeto-relacional do EntityFramework

        public virtual SessaoUsuarioViewModel SessaoUsuario { get; set; } // UML - (1) Usuario é associado com (1) SessaoUsuario. Virtual para lazy load

        public virtual IEnumerable<NotificacaoViewModel> Notificacoes { get; set; } // UML - (0..1) Usuario é associado com (0..*) Notificacao. Virtual para lazy load

        public virtual IEnumerable<HistoricoOperacaoViewModel> HistoricoOperacoes { get; set; } // UML - (1) Usuario é agregado com (0..*) HistoricoOperacao. Virtual para lazy load

        public virtual List<UsuarioViewModel> ListUsuarioViewModel { get; set; }

        public virtual List<FuncionalidadeViewModel> ListFuncionalidadeViewModel { get; set; }

        public string NomeFiltro { get; set; }
        public string EmailFiltro { get; set; }

    }
}