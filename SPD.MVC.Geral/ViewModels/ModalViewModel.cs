using SPD.MVC.Geral.ViewModels;
using System;
using System.Collections.Generic;

namespace SPD.MVC.Geral.ViewModels
{
    /// <summary>
    /// ViewModel base utilizado na exibição de modais no sistema.
    /// </summary>
    [Serializable]
    public class ModalViewModel : ViewModelBase
    {
        public ModalViewModel()
        {
            this.ID = "modal";
            this.Botoes = new List<botaoViewModel>();
        }

        public ModalViewModel(params botaoViewModel[] botoes)
            : this()
        {
            this.Botoes = new List<botaoViewModel>(botoes);
        }

        public const string NAME = "_ModelGenerica";
        public string ID { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public string BtnFecharCancelar { get; set; } = "Cancelar";

        public Dictionary<string, object> Valores { get; set; }
        public List<botaoViewModel> Botoes { get; set; }
        public bool showMensagemErro { get; set; }
        public bool historico { get; set; }
        public bool popupDownload { get; set; }
        public bool popupNovaPosse { get; set; }
        public string Aeronave { get; set; }
        public string Cliente { get; set; }
        public DateTime dt_vigencia { get; set; }
        public string comentario { get; set; }
        public bool useCheck { get; set; }
    }

    public class botaoViewModel
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public string Text { get; set; }
        public string Class { get; set; }
        public string OnClick { get; set; }
        public string Tipo { get; set; } = "button";
    }
}
