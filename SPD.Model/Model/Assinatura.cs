using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Model.Model
{
    public class Assinatura
    {
        public int ID { get; set; }

        public string NOME_RESPONSAVEL { get; set; }

        public string CPF_RESPONSAVEL { get; set; }

        public byte[] ASSINATURA { get; set; }

        public DateTime DT_INSERT { get; set; }
    }
}
