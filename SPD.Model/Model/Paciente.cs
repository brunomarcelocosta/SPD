﻿using SPD.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Model.Model
{
    public class Paciente
    {
        public int ID { set; get; }

        public string NOME { get; set; }

        public string DATA_NASC { get; set; }

        public string CPF { get; set; }

        public string RG { get; set; }

        public string EMAIL { get; set; }

        public string CELULAR { get; set; }

        public string ESTADO_CIVIL { get; set; }

        public string PROFISSAO { get; set; }

        public string END_RUA { get; set; }

        public string END_NUMERO { get; set; }

        public string END_COMPL { get; set; }

        public string CEP { get; set; }

        public string BAIRRO { get; set; }

        public string CIDADE { get; set; }

        public string UF { get; set; }

        public string PAIS { get; set; }

        public byte[] FOTO { get; set; }

        //public bool? INDICACAO { get; set; }

        //public string NOME_INDICACAO { get; set; }

        public bool ATIVO { get; set; }
    }
}
