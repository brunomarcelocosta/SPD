using SPD.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Model.Model
{
    public class HistoricoOperacao
    {
        public int ID { set; get; }

        public string DESCRICAO { get; set; }

        public int ID_USUARIO { get; set; }
        public virtual Usuario USUARIO { get; set; }

        public int ID_TIPO_OPERACAO { get; set; }
        public virtual TipoOperacao TipoOperacao { get; set; }

        public DateTime DT_OPERACAO { get; set; }
    }
}
