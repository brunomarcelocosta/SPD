using SPD.Data.EntityTypeConfigurations;
using SPD.Model.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace SPD.Data.EntityConfigurations.ModelsConfiguration
{
    public class AgendaConfiguration : EntityTypeConfiguration<Agenda>, IConfiguration
    {
        public AgendaConfiguration()
        {
            ToTable("SPD_AGENDA");

            HasKey(ag => ag.ID, opcoes => opcoes.HasName("SPD_AGENDA_PK"));

            Property(ag => ag.ID)
           .HasColumnName("id_agenda")
           .IsRequired();

            Property(ag => ag.ID_DENTISTA)
           .HasColumnName("fk_id_dentista")
           .IsRequired()
           .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("SPD_AGENDA_SPD_DENTISTA_FK")));
            HasRequired(ag => ag.DENTISTA)
           .WithMany()
           .HasForeignKey(ag => ag.ID_DENTISTA);

            Property(ag => ag.ID_PACIENTE)
           .HasColumnName("fk_id_paciente")
           .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("SPD_AGENDA_SPD_PACIENTE_FK")));

            Property(ag => ag.NOME_PACIENTE)
           .HasColumnName("nm_paciente")
           .HasMaxLength(50)
           .IsRequired();

            Property(ag => ag.CELULAR)
           .HasColumnName("celular")
           .HasMaxLength(50)
           .IsRequired();

            Property(pre => pre.DATA_CONSULTA)
           .HasColumnName("dt_consulta")
           .IsRequired();

            Property(pre => pre.HORA_INICIO)
           .HasColumnName("hora_inicio")
           .IsRequired();

            Property(pre => pre.HORA_FIM)
           .HasColumnName("hora_fim")
           .IsRequired();

            Property(ag => ag.ID_USUARIO)
           .HasColumnName("fk_id_usuario")
           .IsRequired()
           .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("SPD_AGENDA_SPD_USUARIO_FK")));
            HasRequired(ag => ag.USUARIO)
           .WithMany()
           .HasForeignKey(ag => ag.ID_USUARIO);

            Property(pre => pre.DT_INSERT)
           .HasColumnName("dt_insert")
           .IsRequired();

            Ignore(ag => ag.PACIENTE);
        }
    }
}
