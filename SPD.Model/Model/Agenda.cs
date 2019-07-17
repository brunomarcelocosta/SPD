using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Model.Model
{
    public class Agenda
    {
        public int ID { get; set; }

        public int ID_DENTISTA { get; set; }
        public virtual Dentista DENTISTA { get; set; }

        public int? ID_PACIENTE { get; set; }
        public virtual Paciente PACIENTE { get; set; }

        public string NOME_PACIENTE { get; set; }

        public DateTime DATA_CONSULTA { get; set; }

        public int ID_USUARIO { get; set; }
        public virtual Usuario USUARIO { get; set; }

        public DateTime DT_INSERT { get; set; }
    }
}
