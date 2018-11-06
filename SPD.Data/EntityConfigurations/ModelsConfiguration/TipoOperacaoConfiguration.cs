using SPD.Data.EntityTypeConfigurations;
using SPD.Model.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Data.EntityConfigurations.ModelsConfiguration
{
    public class TipoOperacaoConfiguration : EntityTypeConfiguration<TipoOperacao>, IConfiguration
    {
        public TipoOperacaoConfiguration()
        {
            this.ToTable("SPD_TIPO_OPERACAO");

            this.HasKey(tipoOperacao => tipoOperacao.ID, opcoes => opcoes.HasName("SPD_TIPO_OPERACAO_PK"));

            this.Property(tipoOperacao => tipoOperacao.ID)
                .HasColumnName("id_tipo_operacao")
                .IsRequired();

            this.Property(tipoOperacao => tipoOperacao.CODIGO_TIPO_OPERACAO)
                .HasColumnName("codigo_operacao")
                .HasMaxLength(15)
                .IsRequired();

            this.Property(tipoOperacao => tipoOperacao.DESCRICAO_TIPO_OPERACAO)
                .HasColumnName("descricao_operacao")
                .HasMaxLength(255);
        }
    }
}
