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
    public class SessaoUsuarioConfiguration : EntityTypeConfiguration<SessaoUsuario>, IConfiguration
    {
        public SessaoUsuarioConfiguration()
        {
            this.ToTable("SPD_SESSAO_USUARIO");

            this.HasKey(sessaoUsuario => sessaoUsuario.ID, opcoes => opcoes.HasName("SPD_SESSAO_USUARIO_PK"));

            this.Property(sessaoUsuario => sessaoUsuario.ID)
                .HasColumnName("id_sessao")
                .IsRequired();

            this.Property(sessaoUsuario => sessaoUsuario.UsuarioID)
                .HasColumnName("fk_id_usuario")
                .IsRequired()
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("SPD_SESSAO_USUARIO_SPD_USUARIO_FK")))
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("SPD_SESSAO_USUARIO__UN") { IsUnique = true }));

            this.HasRequired(sessaoUsuario => sessaoUsuario.Usuario)
                .WithMany()
                .HasForeignKey(sessaoUsuario => sessaoUsuario.UsuarioID);

            this.Property(sessaoUsuario => sessaoUsuario.EnderecoIP)
                .HasColumnName("vl_endereco_ip")
                .HasMaxLength(15)
                .IsRequired();

            this.Property(sessaoUsuario => sessaoUsuario.DataHoraAcesso)
                .HasColumnName("data_hora_acesso")
                .IsRequired();
        }
    }
}
