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

        public virtual ICollection<UsuarioFuncionalidade> UsuarioFuncionalidade { get; set; } // UML - (1..*) Perfil é associado com (0..*) Usuario. Virtual para lazy load

        public IEnumerable<Usuario> Usuarios
        {
            get
            {
                return (this.UsuarioFuncionalidade == null ? new List<Usuario>() : this.UsuarioFuncionalidade.Select(usuariosPerfil => usuariosPerfil.USUARIO).ToList());
            }
        }
    }
}
