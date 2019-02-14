using System;

namespace SPD.Model.Model
{
    public class Dentista
    {
        public int ID { get; set; }

        public string NOME { get; set; }

        public string CRO { get; set; }

        public int ID_USUARIO { get; set; }
        public virtual Usuario USUARIO { get; set; }

        public DateTime DT_INSERT { get; set; }

    }
}
