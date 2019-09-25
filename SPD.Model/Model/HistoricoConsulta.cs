using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Model.Model
{
    public class HistoricoConsulta
    {
        public int ID { get; set; }

        public int ID_CONSULTA { get; set; }
        public virtual Consulta CONSULTA { get; set; }

        public DateTime DT_CONSULTA { get; set; }

        public virtual string DataConsulta { get; set; }
        public virtual string Dentista { get; set; }
        public virtual string Paciente { get; set; }
        public virtual string Descricao { get; set; }

    }
}
