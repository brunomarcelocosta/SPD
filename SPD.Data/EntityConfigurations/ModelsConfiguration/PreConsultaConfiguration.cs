using SPD.Data.EntityTypeConfigurations;
using SPD.Model.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace SPD.Data.EntityConfigurations.ModelsConfiguration
{
    public class PreConsultaConfiguration : EntityTypeConfiguration<PreConsulta>, IConfiguration
    {
        public PreConsultaConfiguration()
        {
            ToTable("SPD_PRE_CONSULTA");

            HasKey(pre => pre.ID, opcoes => opcoes.HasName("SPD_PRE_CONSULTA_PK"));

            Property(pre => pre.ID)
           .HasColumnName("id_pre_consulta")
           .IsRequired();

            Property(pre => pre.ID_AGENDA)
           .HasColumnName("fk_id_agenda")
           .IsRequired()
           .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("SPD_CONSULTA_SPD_PACIENTE_FK")));
            HasRequired(pre => pre.AGENDA)
           .WithMany()
           .HasForeignKey(pre => pre.ID_AGENDA);

            Property(pre => pre.MAIOR_IDADE)
           .HasColumnName("maior_idade")
           .IsRequired();

            Property(pre => pre.AUTORIZADO)
           .HasColumnName("autorizado");

            Property(pre => pre.CONSULTA_INICIADA)
           .HasColumnName("consulta_inic");

            Property(pre => pre.CONVENIO)
           .HasColumnName("convenio")
           .HasMaxLength(50);

            Property(pre => pre.NUMERO_CARTERINHA)
           .HasColumnName("nr_carterinha")
           .HasMaxLength(50);

            Property(pre => pre.ID_ASSINATURA)
           .HasColumnName("fk_id_assinatura")
           .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("SPD_PRE_CONSULTA_SPD_ASSINATURA_FK")));

            Property(pre => pre.DT_INSERT)
           .HasColumnName("dt_insert")
           .IsRequired();

            Ignore(pre => pre.Assinatura);
        }
    }
}
