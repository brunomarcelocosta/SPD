using SPD.Data.EntityTypeConfigurations;
using SPD.Model.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace SPD.Data.EntityConfigurations.ModelsConfiguration
{
    public class ClinicaConfiguration : EntityTypeConfiguration<Clinica>, IConfiguration
    {
        public ClinicaConfiguration()
        {
            ToTable("SPD_CLINICA");

            HasKey(clinica => clinica.ID, opcoes => opcoes.HasName("SPD_CLINICA_PK"));

            Property(clinica => clinica.ID)
           .HasColumnName("id_clinica")
           .IsRequired();

            Property(clinica => clinica.NOME)
           .HasColumnName("nome")
           .HasMaxLength(255)
           .IsRequired();

            Property(clinica => clinica.LOGO)
           .HasColumnName("logo");

            Property(clinica => clinica.ID_USUARIO)
           .HasColumnName("fk_id_usuario")
           .IsRequired()
           .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("SPD_CLINICA_SPD_USUARIO_FK") { IsUnique = true }));
            HasRequired(clinica => clinica.USUARIO)
           .WithMany()
           .HasForeignKey(clinica => clinica.ID_USUARIO);

            Property(clinica => clinica.DT_INSERT)
           .HasColumnName("dt_insert")
           .IsRequired();

        }
    }
}
