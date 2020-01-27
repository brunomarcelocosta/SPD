using System;

namespace SPD.Model.Model
{
    public class Clinica
    {
        public int ID { get; set; }

        public string NOME { get; set; }

        public byte[] LOGO { get; set; }

        public int ID_USUARIO { get; set; }
        public virtual Usuario USUARIO { get; set; }

        public DateTime DT_INSERT { get; set; }
    }
}
