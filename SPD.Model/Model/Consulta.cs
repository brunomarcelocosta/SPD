using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Model.Model
{
    public class Consulta
    {
        public int ID { get; set; }

        public int ID_DENTISTA { get; set; }
        public virtual Dentista DENTISTA { get; set; }

        public int ID_PRE_CONSULTA { get; set; }
        public virtual PreConsulta PRE_CONSULTA { get; set; }

        public string DESCRICAO_PROCEDIMENTO { get; set; }

        public byte[] ODONTOGRAMA { get; set; }

        public byte[] EXAME { get; set; }

        public DateTime DT_CONSULTA { get; set; }

        //public bool EXCLUIDO { get; set; } // 0 - NAO       1 -  SIM 
    }
}
