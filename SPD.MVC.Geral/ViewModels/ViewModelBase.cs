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
        public virtual SelectList ListAgendaDia { get; set; }


        public string FieldSort { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DataDe { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DataAte { get; set; }

        public string DataDe_Filtro { get; set; }
        public string DataAte_Filtro { get; set; }
        public string HoraDe_Filtro { get; set; }
        public string HoraAte_Filtro { get; set; }
        public string Descricao_Filtro { get; set; }

        public string TipoOperacao_Filtro { get; set; }
        public string Usuario_Filtro { get; set; }
        public string Funcionalidade_Filtro { get; set; }

        public List<string> ListHorarios()
        {
            return new List<string>()
            {
                "08:00",
                "08:15",
                "08:30",
                "08:45",
                "09:00",
                "09:15",
                "09:30",
                "09:45",
                "10:00",
                "10:15",
                "10:30",
                "10:45",
                "11:00",
                "11:15",
                "11:30",
                "11:45",
                "12:00",
                "12:15",
                "12:30",
                "12:45",
                "13:00",
                "13:15",
                "13:30",
                "13:45",
                "14:00",
                "14:15",
                "14:30",
                "14:45",
                "15:00",
                "15:15",
                "15:30",
                "15:45",
                "16:00",
                "16:15",
                "16:30",
                "16:45",
                "17:00",
                "17:15",
                "17:30",
                "17:45",
                "18:00",
                "18:15",
                "18:30",
                "18:45",
                "19:00",
                "19:15",
                "19:30",
                "19:45",
                "20:00"
            };
        }

    }
}