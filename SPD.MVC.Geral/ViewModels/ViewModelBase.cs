using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SPD.MVC.Geral.ViewModels
{
    /// <summary>
    /// Viewmodel base para adição de campos comuns.
    /// </summary>
    [Serializable]
    public class ViewModelBase
    {

        public virtual SelectList ListFuncionalidade { get; set; }
        public virtual SelectList ListUsuario { get; set; }
        public virtual SelectList ListTipoOperacao { get; set; }
        public virtual SelectList ListEstadoCivil { get; set; }
        public virtual SelectList ListNomePaciente { get; set; }
        public virtual SelectList ListNomeDentista { get; set; }


        public string FieldSort { get; set; }
        public int PageNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DataDe { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DataAte { get; set; }

        public string DataDe_Filtro { get; set; }
        public string DataAte_Filtro { get; set; }
        public string Descricao_Filtro { get; set; }

        public string TipoOperacao_Filtro { get; set; }
        public string Usuario_Filtro { get; set; }
        public string Funcionalidade_Filtro { get; set; }



    }
}