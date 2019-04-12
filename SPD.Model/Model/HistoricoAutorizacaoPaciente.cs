using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Model.Model
{
    public class HistoricoAutorizacaoPaciente
    {
        public int ID { get; set; }

        public int ID_PACIENTE { get; set; }
        public virtual Paciente PACIENTE { get; set; }

        public int ID_ASSINATURA { get; set; }
        public virtual Assinatura ASSINATURA { get; set; }

        public DateTime DT_INSERT { get; set; }
    }
}
