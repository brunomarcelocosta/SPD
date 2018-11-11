using SPD.Data.EntityTypeConfigurations;
using SPD.Model.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace SPD.Infrastructure.Data.EntityConfigurations.Administracao
{
    /// <summary>
    /// Classe responsável por criar a tabela  ADM_historico_operacao no banco de dados.
    /// </summary>
    public class HistoricoOperacaoConfiguration : EntityTypeConfiguration<HistoricoOperacao>, IConfiguration
    {
        public HistoricoOperacaoConfiguration()
        {
            this.ToTable("SPD_HISTORICO_OPERACAO");

            this.HasKey(historicoOperacao => historicoOperacao.ID, opcoes => opcoes.HasName("SPD_HISTORICO_OPERACAO_PK"));

            this.Property(historicoOperacao => historicoOperacao.ID)
                .HasColumnName("id_historico_operacao")
                .IsRequired();

            this.Property(historicoOperacao => historicoOperacao.ENDERECO_IP)
                .HasColumnName("endereco_ip")
                .HasMaxLength(15);

            this.Property(historicoOperacao => historicoOperacao.DESCRICAO)
                .HasColumnName("descricao")
                .HasMaxLength(4000)
                .IsRequired();

            this.Property(historicoOperacao => historicoOperacao.DT_OPERACAO)
                .HasColumnName("dt_operacao")
                .IsRequired();

           this.Property(historicoOperacao => historicoOperacao.ID_TIPO_OPERACAO)
               .HasColumnName("fk_id_tipo_operacao")
               .IsRequired()
               .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("SPD_HISTORICO_OPERACAO_SPD_TIPO_OPERACAO")));
           this.HasRequired(historicoOperacao => historicoOperacao.tipoOperacao)
               .WithMany()
               .HasForeignKey(historicoOperacao => historicoOperacao.ID_TIPO_OPERACAO);

            this.Property(historicoOperacao => historicoOperacao.ID_USUARIO)
                .HasColumnName("fk_id_usuario")
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("SPD_HISTORICO_OPERACAO_SPD_USUARIO_FK")));
            this.HasRequired(historicoOperacao => historicoOperacao.USUARIO)
                .WithMany()
                .HasForeignKey(historicoOperacao => historicoOperacao.ID_USUARIO);

            this.Property(historicoOperacao => historicoOperacao.ID_FUNCIONALIDADE)
                .HasColumnName("fk_id_funcionalidade")
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("SPD_HISTORICO_OPERACAO_SPD_FUNCIONALIDADE_FK")));
            this.HasRequired(historicoOperacao => historicoOperacao.FUNCIONALIDADE)
                .WithMany()
                .HasForeignKey(historicoOperacao => historicoOperacao.ID_FUNCIONALIDADE);

            this.Property(historicoOperacao => historicoOperacao.DUMP)
                .HasColumnName("de_dump")
                .HasMaxLength(4000);

        }
    }
}
