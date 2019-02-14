using SPD.Data.EntityTypeConfigurations;
using SPD.Model.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace SPD.Data.EntityConfigurations.ModelsConfiguration
{
    public class PacienteConfiguration : EntityTypeConfiguration<Paciente>, IConfiguration
    {
        public PacienteConfiguration()
        {
            ToTable("SPD_PACIENTE");

            HasKey(paciente => paciente.ID, opcoes => opcoes.HasName("SPD_PACIENTE_PK"));

            Property(paciente => paciente.ID)
           .HasColumnName("id_paciente")
           .IsRequired();

            Property(paciente => paciente.NOME)
           .HasColumnName("nome")
           .HasMaxLength(255)
           .IsRequired();

            Property(paciente => paciente.DATA_NASC)
           .HasColumnName("dt_nasc")
           .HasMaxLength(10)
           .IsRequired();

            Property(paciente => paciente.CPF)
           .HasColumnName("cpf")
           .HasMaxLength(50)
           .IsRequired()
           .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute() { IsUnique = true }));

            Property(paciente => paciente.RG)
           .HasColumnName("rg")
           .HasMaxLength(50);

            Property(paciente => paciente.EMAIL)
           .HasColumnName("email")
           .HasMaxLength(50);

            Property(paciente => paciente.CELULAR)
           .HasColumnName("celular")
           .HasMaxLength(15);

            Property(paciente => paciente.ESTADO_CIVIL)
           .HasColumnName("estado_civil")
           .HasMaxLength(50);

            Property(paciente => paciente.PROFISSAO)
           .HasColumnName("profissao")
           .HasMaxLength(50);

            Property(paciente => paciente.END_RUA)
           .HasColumnName("end_rua")
           .HasMaxLength(50);

            Property(paciente => paciente.END_NUMERO)
           .HasColumnName("end_numero")
           .HasMaxLength(6);

            Property(paciente => paciente.END_COMPL)
           .HasColumnName("end_compl")
           .HasMaxLength(50);

            Property(paciente => paciente.CEP)
           .HasColumnName("cep")
           .HasMaxLength(50);

            Property(paciente => paciente.BAIRRO)
           .HasColumnName("bairro")
           .HasMaxLength(50);

            Property(paciente => paciente.CIDADE)
           .HasColumnName("cidade")
           .HasMaxLength(50);

            Property(paciente => paciente.UF)
           .HasColumnName("uf")
           .HasMaxLength(2);

            Property(paciente => paciente.PAIS)
           .HasColumnName("pais")
           .HasMaxLength(50);

            Property(paciente => paciente.TIPO_PACIENTE)
           .HasColumnName("tipo_paciente")
           .IsRequired();

            Property(paciente => paciente.FOTO)
           .HasColumnName("foto");

            Property(paciente => paciente.ATIVO)
           .HasColumnName("lg_ativo")
           .IsRequired();
        }
    }
}
