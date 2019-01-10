using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Model.Model
{
    public class UsuarioFuncionalidade
    {
        public int ID { get; set; }

        public int ID_USUARIO { get; set; }
        public virtual Usuario Usuario { get; set; }

        public int ID_FUNCIONALIDADE { get; set; }
        public virtual Funcionalidade Funcionalidade { get; set; }
    }
}
