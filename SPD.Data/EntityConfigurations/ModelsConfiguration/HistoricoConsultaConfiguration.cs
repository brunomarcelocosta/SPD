using SPD.Data.EntityTypeConfigurations;
using SPD.Model.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace SPD.Data.EntityConfigurations.ModelsConfiguration
{
    public class HistoricoConsultaConfiguration : EntityTypeConfiguration<HistoricoConsulta>, IConfiguration
    {
        public HistoricoConsultaConfiguration()
        {
            ToTable("SPD_HISTORICO_CONSULTA");

            HasKey(hist => hist.ID, opcoes => opcoes.HasName("SPD_HISTORICO_CONSULTA_PK"));

            Property(hist => hist.ID)
           .HasColumnName("id_historico_consulta")
           .IsRequired();

            Property(hist => hist.ID_CONSULTA)
           .HasColumnName("fk_id_consulta")
           .IsRequired()
           .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("SPD_HISTORICO_CONSULTA_SPD_CONSULTA_FK")));
            HasRequired(hist => hist.CONSULTA)
           .WithMany()
           .HasForeignKey(hist => hist.ID_CONSULTA);

            Property(hist => hist.DT_CONSULTA)
           .HasColumnName("dt_consulta")
           .IsRequired();
        }
    }
}
