﻿using SPD.MVC.Geral.ViewModels;
using System;
using System.Collections.Generic;

namespace SPD.MVC.PortalWeb.ViewModels
{
    public class PreConsultaViewModel : ViewModelBase
    {
        public int ID { get; set; }

        public int ID_Paciente { get; set; }
        public virtual PacienteViewModel Paciente { get; set; }

        public bool Maior_Idade { get; set; }

        public bool? Autorizado { get; set; }

        public byte[] Autorizacao { get; set; }

        public string Convenio { get; set; }

        public string Numero_Carterinha { get; set; }

        public int? ID_Assinatura { get; set; }
        public AssinaturaViewModel Assinatura { get; set; }

        public string Tipo_Pagamento { get; set; }

        public DateTime Dt_Insert { get; set; }

        public List<PreConsultaViewModel> ListPreConsultaViewModel { get; set; }
        public string Paciente_string { get; set; }
        public string Autorizado_string { get; set; }
        public bool Conveniado { get; set; }
        public string Idade { get; set; }
        public bool particular { get; set; }
        public string Nome_Responsavel { get; set; }
        public string Cpf_Responsavel { get; set; }
        public byte Ass_Responsavel { get; set; }
    }
}