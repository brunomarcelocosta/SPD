using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Model.Model
{
    public sealed class Context
    {
        public int usuarioID { get; set; }
        public string usuarioIP { get; set; }
        public string usuarioSessionID { get; set; }

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "Context: UsuarioID: \"{0}\" UsuarioIP: \"{1}\" UsuarioSessionID: \"{2}\"", this.usuarioID, this.usuarioIP, this.usuarioSessionID);
        }
    }
}
