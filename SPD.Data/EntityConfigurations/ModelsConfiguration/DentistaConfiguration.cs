using SPD.Data.EntityTypeConfigurations;
using SPD.Model.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace SPD.Data.EntityConfigurations.ModelsConfiguration
{
    public class DentistaConfiguration :  EntityTypeConfiguration<Dentista>, IConfiguration
    {
        public DentistaConfiguration()
        {
            ToTable("SPD_DENTISTA");

            HasKey(dentista => dentista.ID, opcoes => opcoes.HasName("SPD_DENTISTA_PK"));

            Property(dentista => dentista.ID)
           .HasColumnName("id_dentista")
           .IsRequired();

            Property(dentista => dentista.NOME)
           .HasColumnName("nome")
           .HasMaxLength(50)
           .IsRequired();

            Property(dentista => dentista.CRO)
           .HasColumnName("cro")
           .HasMaxLength(50)
           .IsRequired();

            Property(dentista => dentista.ID_CLINICA)
           .HasColumnName("fk_id_clinica")
           .IsRequired()
           .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("SPD_DENTISTA_SPD_USUARIO_FK")));
            HasRequired(dentista => dentista.CLINICA)
           .WithMany()
           .HasForeignKey(dentista => dentista.ID_CLINICA);

            Property(dentista => dentista.DT_INSERT)
           .HasColumnName("dt_insert")
           .IsRequired();
        }
    }
}
