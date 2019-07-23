using System;
using System.Collections.Generic;
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

        public List<string> ListHorarios()
        {
            return new List<string>()
            {
                "08:00",
                "08:30",
                "09:00",
                "09:30",
                "10:00",
                "10:30",
                "11:00",
                "11:30",
                "12:00",
                "12:30",
                "13:00",
                "13:30",
                "14:00",
                "14:30",
                "15:00",
                "15:30",
                "16:00",
                "16:30",
                "17:00",
                "17:30",
                "18:00",
                "18:30",
                "19:00",
                "19:30",
                "20:00"
            };
        }

    }
}