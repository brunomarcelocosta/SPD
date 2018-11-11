using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Model.Model
{
    public class SessaoUsuario
    {
        public int ID { get; set; }

        public DateTime dataHoraAcesso { get; set; }

        public string enderecoIP { get; set; }

        public virtual Usuario usuario { get; set; }

        public int ID_USUARIO { get; set; }

        #region Métodos

        public SessaoUsuario()
        {
            this.usuario = null;
        }

        public SessaoUsuario(Usuario usuario)
            : this()
        {
            // ToDo: Must fix: An entity object cannot be referenced by multiple instances of IEntityChangeTracker when we do the relationship via objects.
            //this.Usuario = usuario;
            this.ID_USUARIO = usuario.ID;
        }

        public SessaoUsuario(Usuario usuario, string enderecoIP)
            : this(usuario)
        {
            this.enderecoIP = enderecoIP;
        }

        public void CriarSessao(Context context)
        {
            this.dataHoraAcesso = DateTime.Now;

            if (String.IsNullOrWhiteSpace(this.enderecoIP))
            {
                this.enderecoIP = context.usuarioIP;
            }
        }

        public bool EncerrarSessao(Context context)
        {
            this.dataHoraAcesso = DateTime.Now;

            if (String.IsNullOrWhiteSpace(this.enderecoIP))
            {
                this.enderecoIP = context.usuarioIP;
            }

            return true;
        }

        #endregion
    }
}
