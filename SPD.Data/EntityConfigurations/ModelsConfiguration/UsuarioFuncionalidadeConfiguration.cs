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
    public class UsuarioFuncionalidadeConfiguration : EntityTypeConfiguration<UsuarioFuncionalidade>, IConfiguration
    {
        public UsuarioFuncionalidadeConfiguration()
        {
            this.ToTable("SPD_USUARIO_FUNCIONALIDADE");

            this.HasKey(user => user.ID, opcoes => opcoes.HasName("SPD_USUARIO_FUNCIONALIDADE_PK"));

            this.Property(user => user.ID)
                .HasColumnName("id")
                .IsRequired();

            this.Property(user => user.ID_USUARIO)
                 .HasColumnName("fk_id_usuario")
                 .IsRequired()
                 .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("SPD_USUARIO_FUNCIONALIDADE_SPD_USUARIO_FK")));
            this.HasRequired(user => user.USUARIO)
                .WithMany()
                .HasForeignKey(user => user.ID_USUARIO);

            this.Property(user => user.ID_FUNCIONALIDADE)
                .HasColumnName("fk_id_funcionalidade")
                .IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("SPD_USUARIO_FUNCIONALIDADE_SPD_FUNCIONALIDADE_FK")));
            this.HasRequired(user => user.FUNCIONALIDADE)
                .WithMany()
                .HasForeignKey(user => user.ID_FUNCIONALIDADE);
        }
    }
}
