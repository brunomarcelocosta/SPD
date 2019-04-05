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
    public class AssinaturaConfiguration : EntityTypeConfiguration<Assinatura>, IConfiguration
    {
        public AssinaturaConfiguration()
        {
            ToTable("SPD_ASSINATURA");

            HasKey(consulta => consulta.ID, opcoes => opcoes.HasName("SPD_ASSINATURA_PK"));

            Property(assinatura => assinatura.ID)
           .HasColumnName("id_assinatura")
           .IsRequired();

            Property(assinatura => assinatura.NOME_RESPONSAVEL)
           .HasColumnName("nm_responsavel")
           .HasMaxLength(255)
           .IsRequired();

            Property(assinatura => assinatura.CPF_RESPONSAVEL)
           .HasColumnName("cpf_responsavel")
           .HasMaxLength(25)
           .IsRequired();

            Property(assinatura => assinatura.ASSINATURA)
           .HasColumnName("assinatura")
           .IsRequired();

            Property(assinatura => assinatura.DT_INSERT)
           .HasColumnName("dt_insert")
           .IsRequired();
        }
    }
}
