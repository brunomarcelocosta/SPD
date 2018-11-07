using SPD.Data.EntityTypeConfigurations;
using SPD.Model.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Data.EntityConfigurations.ModelsConfiguration
{
    public class UsuarioConfiguration : EntityTypeConfiguration<Usuario>, IConfiguration
    {
        public UsuarioConfiguration()
        {
            this.ToTable("SPD_USUARIO");

            this.HasKey(usuario => usuario.ID, opcoes => opcoes.HasName("SPD_USUARIO_PK"));

            this.Property(usuario => usuario.ID)
                .HasColumnName("id_usuario")
                .IsRequired();

            this.Property(usuario => usuario.Nome)
                .HasColumnName("nome_usuario")
                .HasMaxLength(255)
                .IsRequired();

            this.Property(usuario => usuario.Email)
                .HasColumnName("email_usuario")
                .HasMaxLength(50)
                .IsRequired();

            this.Property(usuario => usuario.Login)
                .HasColumnName("login_usuario")
                .HasMaxLength(25)
                .IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute() { IsUnique = true }));

            this.Property(usuario => usuario.Password)
                .HasColumnName("senha_usuario")
                .HasMaxLength(50);

            this.Property(usuario => usuario.isAtivo)
                .HasColumnName("status_usuario")
                .IsRequired();

            this.Property(usuario => usuario.isBloqueado)
                .HasColumnName("usuario_bloqueado")
                .IsRequired();

            this.Property(usuario => usuario.TentativasLogin)
                .HasColumnName("tentativas")
                .IsRequired();

            this.Property(usuario => usuario.TrocaSenhaObrigatoria)
                .HasColumnName("lg_troca_obrigatoria");
        }
    }
}
