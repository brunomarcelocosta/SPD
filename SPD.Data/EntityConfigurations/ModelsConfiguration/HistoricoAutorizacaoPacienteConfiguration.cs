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
    public class HistoricoAutorizacaoPacienteConfiguration : EntityTypeConfiguration<HistoricoAutorizacaoPaciente>, IConfiguration
    {
        public HistoricoAutorizacaoPacienteConfiguration()
        {
            ToTable("SPD_HISTORICO_AUTORIZACAO");

            HasKey(hist => hist.ID, opcoes => opcoes.HasName("SPD_HISTORICO_AUTORIZACAO_PK"));

            Property(hist => hist.ID)
           .HasColumnName("id_historico")
           .IsRequired();

            Property(hist => hist.ID_PACIENTE)
           .HasColumnName("fk_id_paciente")
           .IsRequired()
           .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("SPD_HISTORICO_AUTORIZACAO_SPD_PACIENTE_FK")));
            HasRequired(hist => hist.PACIENTE)
           .WithMany()
           .HasForeignKey(hist => hist.ID_PACIENTE);

            Property(hist => hist.ID_ASSINATURA)
           .HasColumnName("fk_id_assinatura")
           .IsRequired()
           .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("SPD_HISTORICO_AUTORIZACAO_SPD_ASSINATURA_FK")));
            HasRequired(hist => hist.ASSINATURA)
           .WithMany()
           .HasForeignKey(hist => hist.ID_ASSINATURA);

            Property(hist => hist.DT_INSERT)
           .HasColumnName("dt_insert")
           .IsRequired();
        }
    }
}
