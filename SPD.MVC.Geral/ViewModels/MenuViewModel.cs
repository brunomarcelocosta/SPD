using System.Collections.Generic;

namespace SPD.MVC.Geral.ViewModels
{
    /// <summary>
    /// Viewmodel para exibição e armazenamento de dados do menu do sistema.
    /// </summary>
    public class MenuViewModel
    {
        public int MenuId { get; set; }
        public int Hierarquia { get; set; }
        public string Titulo { get; set; }
        public bool IsSubMenu { get; set; }
        public string Controle { get; set; }
        public string Acao { get; set; }
        public string Modulo { get; set; }
        public string Perfil { get; set; }
        public virtual List<MenuViewModel> SubMenus { get; set; }
    }
}
