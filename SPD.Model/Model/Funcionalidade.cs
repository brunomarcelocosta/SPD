using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Model.Model
{
    public class Funcionalidade
    {
        public int ID { get; set; } // Alterado para public devido ao mapeamento objeto-relacional do EntityFramework

        public string Nome { get; set; }

        public bool isAtivo { get; set; }
    }
}
