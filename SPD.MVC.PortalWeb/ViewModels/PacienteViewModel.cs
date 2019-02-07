using SPD.MVC.Geral.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SPD.MVC.PortalWeb.ViewModels
{
    public class PacienteViewModel : ViewModelBase
    {
        public int ID { set; get; }

        public string Nome { get; set; }

        [DataType(DataType.Date)]
        public DateTime Data_Nasc { get; set; }

        public string Cpf { get; set; }

        public string Rg { get; set; }

        public string Email { get; set; }

        public string Celular { get; set; }

        public string Estado_Civil { get; set; }

        public string Profissao { get; set; }

        public string End_rua { get; set; }

        public string End_Numero { get; set; }

        public string End_Compl { get; set; }

        public string Cep { get; set; }

        public string Bairro { get; set; }

        public string Cidade { get; set; }

        public string Uf { get; set; }

        public string Pais { get; set; }

        public byte[] Foto { get; set; }

        public bool? Tipo_Paciente { get; set; }

        //public bool? INDICACAO { get; set; }

        //public string NOME_INDICACAO { get; set; }

        public bool Ativo { get; set; }

        public List<PacienteViewModel> ListPacienteViewModel { get; set; }

        public string isAtivoFiltro { get; set; }
        public string tipoPacienteFiltro { get; set; }
        public bool particular { get; set; }
        public bool conveniado { get; set; }
        public string srcImage { get; set; }
        public bool ExisteImg { get; set; }
        public string DataNasc_String { get; set; }
    }
}