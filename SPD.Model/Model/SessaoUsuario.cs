using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Model.Model
{
    public class SessaoUsuario
    {
        public int ID { get; set; } // Alterado para public devido ao mapeamento objeto-relacional do EntityFramework
        public DateTime DataHoraAcesso { get; set; }
        public string EnderecoIP { get; set; }
        public virtual Usuario Usuario { get; set; } // UML - (1) SessaoUsuario é associado com (1) Usuario. Virtual para lazy load
        public int UsuarioID { get; set; } // Adicionado devido ao mapeamento objeto-relacional do EntityFramework

        // Adicionado devido ao mapeamento objeto-relacional do EntityFramework
        public SessaoUsuario()
        {
            this.Usuario = null;
        }

        public SessaoUsuario(Usuario usuario)
            : this()
        {
            // ToDo: Must fix: An entity object cannot be referenced by multiple instances of IEntityChangeTracker when we do the relationship via objects.
            //this.Usuario = usuario;
            this.UsuarioID = usuario.ID;
        }

        public SessaoUsuario(Usuario usuario, string enderecoIP)
            : this(usuario)
        {
            this.EnderecoIP = enderecoIP;
        }

        public void CriarSessao(Context context)
        {
            this.DataHoraAcesso = DateTime.Now;

            if (String.IsNullOrWhiteSpace(this.EnderecoIP))
            {
                this.EnderecoIP = context.usuarioIP;
            }
        }

        public bool EncerrarSessao(Context context)
        {
            this.DataHoraAcesso = DateTime.Now;

            if (String.IsNullOrWhiteSpace(this.EnderecoIP))
            {
                this.EnderecoIP = context.usuarioIP;
            }

            return true;
        }
    }
}
