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

        public string NOME { get; set; }

        //public bool IsATIVO { get; set; }

        public virtual ICollection<UsuarioFuncionalidade> usuarioFuncionalidade { get; set; } // UML - (1..*) Perfil é associado com (0..*) Usuario. Virtual para lazy load

        public IEnumerable<Usuario> USUARIOS
        {
            get
            {
                return (this.usuarioFuncionalidade == null ? new List<Usuario>() : this.usuarioFuncionalidade.Select(usuariosPerfil => usuariosPerfil.Usuario).ToList());
            }
        }
    }
}
