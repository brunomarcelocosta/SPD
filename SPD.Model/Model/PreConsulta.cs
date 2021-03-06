﻿using System;

namespace SPD.Model.Model
{
    public class PreConsulta
    {
        public int ID { get; set; }

        public int ID_AGENDA { get; set; }
        public virtual Agenda AGENDA { get; set; }

        public bool MAIOR_IDADE { get; set; }

        public bool? AUTORIZADO { get; set; }

        public bool? CONSULTA_INICIADA { get; set; }

        public string CONVENIO { get; set; }

        public string NUMERO_CARTERINHA { get; set; }

        public int? ID_ASSINATURA { get; set; }
        public virtual Assinatura Assinatura { get; set; }

        //public string VALOR_CONSULTA { get; set; }

        //public string TIPO_PAGAMENTO { get; set; }

        public DateTime DT_INSERT { get; set; }
    }
}
