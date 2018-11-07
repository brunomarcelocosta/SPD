using SPD.MVC.Geral.Content.Texts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPD.MVC.Geral.ViewModels
{
    [Serializable]
    public class AuthenticationViewModel : ViewModelBase
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "NomeRequiredMessage")]
        [Display(ResourceType = typeof(AuthenticationResource), Name = "Nome")]
        public string Nome { get; set; }

        // Não está sendo utilizado pois a mensagem é exibida de forma padrão na view
        //[Required(ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "LoginRequiredMessage")]
        [Required]
        [Display(ResourceType = typeof(AuthenticationResource), Name = "Login")]
        public string Login { get; set; }

        // Não está sendo utilizado pois a mensagem é exibida de forma padrão na view
        //[Required(ErrorMessageResourceType = typeof(AuthenticationResource), ErrorMessageResourceName = "PasswordRequiredMessage")]
        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(AuthenticationResource), Name = "Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(AuthenticationResource), Name = "DataAcesso")]
        public DateTime? DataAcesso { get; set; }

        [Display(ResourceType = typeof(AuthenticationResource), Name = "TrocaSenhaObrigatoria")]
        public bool TrocaSenhaObrigatoria { get; set; }

        [Display(ResourceType = typeof(AuthenticationResource), Name = "EnderecoIP")]
        public string EnderecoIP { get; set; }

        // Representa o identificador da tabela SessaoUsuario
        [Display(ResourceType = typeof(AuthenticationResource), Name = "SessionID")]
        public int SessionID { get; set; }

        [Display(ResourceType = typeof(AuthenticationResource), Name = "PerfilUsuarioIDs")]
        public List<int> PerfilUsuarioIDs = new List<int>();

        [Display(ResourceType = typeof(AuthenticationResource), Name = "PerfilUsuarioNomes")]
        public List<string> PerfilUsuarioNomes = new List<string>();

    }
}
