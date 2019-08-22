using SPD.Data.EntityTypeConfigurations;
using SPD.Model.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace SPD.Data.EntityConfigurations.ModelsConfiguration
{
    public class ConsultaConfiguration : EntityTypeConfiguration<Consulta>, IConfiguration
    {
        public ConsultaConfiguration()
        {
            ToTable("SPD_CONSULTA");

            HasKey(consulta => consulta.ID, opcoes => opcoes.HasName("SPD_CONSULTA_PK"));

            Property(consulta => consulta.ID)
           .HasColumnName("id_consulta")
           .IsRequired();

            Property(consulta => consulta.ID_DENTISTA)
           .HasColumnName("fk_id_dentista")
           .IsRequired()
           .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("SPD_CONSULTA_SPD_DENTISTA_FK")));
            HasRequired(consulta => consulta.DENTISTA)
           .WithMany()
           .HasForeignKey(consulta => consulta.ID_DENTISTA);

            Property(consulta => consulta.ID_PRE_CONSULTA)
           .HasColumnName("fk_id_pre_consulta")
           .IsRequired()
           .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("SPD_CONSULTA_SPD_PRE_CONSULTA_FK")));
            HasRequired(consulta => consulta.PRE_CONSULTA)
           .WithMany()
           .HasForeignKey(consulta => consulta.ID_PRE_CONSULTA);

            Property(consulta => consulta.DESCRICAO_PROCEDIMENTO)
           .HasColumnName("desc_procedimento")
           .HasMaxLength(255)
           .IsRequired();

            Property(consulta => consulta.ODONTOGRAMA)
           .HasColumnName("odontograma");

            Property(consulta => consulta.EXAME)
           .HasColumnName("exame");

            Property(consulta => consulta.COMENTARIOS)
           .HasColumnName("comentarios")
           .HasMaxLength(null)
           .IsRequired();

            Property(consulta => consulta.DT_CONSULTA)
           .HasColumnName("dt_consulta")
           .IsRequired();

        }
    }
}
