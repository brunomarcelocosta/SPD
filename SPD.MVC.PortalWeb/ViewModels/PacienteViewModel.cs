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

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Data_Nasc { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Rg { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Celular { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Estado_Civil { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Profissao { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string End_rua { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string End_Numero { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string End_Compl { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Cep { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Uf { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Pais { get; set; }

        public byte[] Foto { get; set; }

        //public bool? INDICACAO { get; set; }

        //public string NOME_INDICACAO { get; set; }

        public bool Ativo { get; set; }

        public List<PacienteViewModel> ListPacienteViewModel { get; set; }

        public string isAtivoFiltro { get; set; }
        public string tipoPacienteFiltro { get; set; }
        public string srcImage { get; set; }
        public bool ExisteImg { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Agenda_Dia { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Horario { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        public string Nome_Paciente { get; set; }

    }
}