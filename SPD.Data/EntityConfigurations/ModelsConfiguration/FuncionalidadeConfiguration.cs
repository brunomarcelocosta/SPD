using SPD.Data.EntityTypeConfigurations;
using SPD.Model.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace SPD.Data.EntityConfigurations.Administracao
{
    public class FuncionalidadeConfiguration : EntityTypeConfiguration<Funcionalidade>, IConfiguration
    {
        public FuncionalidadeConfiguration()
        {
            this.ToTable("SPD_FUNCIONALIDADE");

            this.HasKey(funcionalidade => funcionalidade.ID, opcoes => opcoes.HasName("SPD_FUNCIONALIDADE__PK"));

            this.Property(funcionalidade => funcionalidade.ID)
                .HasColumnName("id_funcionalidade")
                .IsRequired();

            this.Property(funcionalidade => funcionalidade.Nome)
                .HasColumnName("descricao")
                .HasMaxLength(255)
                .IsRequired();
                //.HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute() { IsUnique = true }));

            this.Property(funcionalidade => funcionalidade.isAtivo)
                .HasColumnName("lg_ativo")
                .IsRequired();

        }
    }
}
