using SPD.Model.Enums;
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

        public DateTime DATA_NASC { get; set; }

        public string CPF { get; set; }

        public string RG { get; set; }

        public string EMAIL { get; set; }

        public string DDD { get; set; }

        public string CELULAR { get; set; }

        public int ID_ESTADO_CIVIL { get; set; }
        public virtual EstadoCivil estadoCivil { get; set; }

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

        public bool? TIPO_PACIENTE { get; set; }

        //public bool? INDICACAO { get; set; }

        //public string NOME_INDICACAO { get; set; }

        public bool ATIVO { get; set; }       
    }
}
