﻿using System;

namespace SPD.Model.Model
{
    public class Dentista
    {
        public int ID { get; set; }

        public string NOME { get; set; }

        public string CRO { get; set; }

        public int ID_CLINICA { get; set; }
        public virtual Clinica CLINICA { get; set; }

        public DateTime DT_INSERT { get; set; }

    }
}
