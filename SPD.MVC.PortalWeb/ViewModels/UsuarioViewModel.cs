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
        [Display(ResourceType = typeof(UsuarioResource), Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "EmailRequiredMessage")]
        [MaxLength(50, ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "EmailMaxLengthMessage")]
        [EmailAddress(ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "EmailEmailAddressMessage", ErrorMessage = null)]
        [Display(ResourceType = typeof(UsuarioResource), Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "LoginRequiredMessage")]
        [MaxLength(25, ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "LoginMaxLengthMessage")]
        [MinLength(8, ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "LoginMinLengthMessage")]
        [Display(ResourceType = typeof(UsuarioResource), Name = "Login")]
        public string Login { get; set; }

        [Required(ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "PasswordRequiredMessage")]
        [MaxLength(50, ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "PasswordMaxLengthMessage")]
        [MinLength(8, ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "PasswordMinLengthMessage")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "PasswordRule1Message")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(UsuarioResource), Name = "Password")]
        public string Password { get; set; } // Alterado para public devido ao mapeamento objeto-relacional do EntityFramework

        // ToDo: Adicionado para o cadastro. Talvez remover.
        [MaxLength(50, ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "PasswordMaxLengthMessage")]
        [Compare("Password", ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "PasswordConfirmacaoCompareMessage")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(UsuarioResource), Name = "PasswordConfirmacao")]
        public string PasswordConfirmacao { get; set; }

        [Display(ResourceType = typeof(UsuarioResource), Name = "TentativasLogin")]
        public int TentativasLogin { get; set; } // Alterado para public devido ao mapeamento objeto-relacional do EntityFramework

        [Required(ErrorMessageResourceType = typeof(UsuarioResource), ErrorMessageResourceName = "isAtivoRequiredMessage")]
        [Display(ResourceType = typeof(UsuarioResource), Name = "isAtivo")]
        public bool isAtivo { get; set; }

        [Display(ResourceType = typeof(UsuarioResource), Name = "isBloqueado")]
        public bool isBloqueado { get; set; }

        [Display(ResourceType = typeof(UsuarioResource), Name = "UltimaTrocaSenha")]
        public DateTime UltimaTrocaSenha { get; set; } // Alterado para public devido ao mapeamento objeto-relacional do EntityFramework

        [Display(ResourceType = typeof(UsuarioResource), Name = "TrocaSenhaObrigatoria")]
        public bool TrocaSenhaObrigatoria { get; set; } // Alterado para public devido ao mapeamento objeto-relacional do EntityFramework

        public virtual SessaoUsuarioViewModel SessaoUsuario { get; set; } // UML - (1) Usuario é associado com (1) SessaoUsuario. Virtual para lazy load

        public virtual IEnumerable<NotificacaoViewModel> Notificacoes { get; set; } // UML - (0..1) Usuario é associado com (0..*) Notificacao. Virtual para lazy load

        public virtual IEnumerable<HistoricoOperacaoViewModel> HistoricoOperacoes { get; set; } // UML - (1) Usuario é agregado com (0..*) HistoricoOperacao. Virtual para lazy load
        public virtual IList<FuncionalidadeViewModel> Funcionalidades { get; set; } //UML - (0..*) Perfil é associado com (1..*) Funcionalidade. Virtual para lazy load

    }
}