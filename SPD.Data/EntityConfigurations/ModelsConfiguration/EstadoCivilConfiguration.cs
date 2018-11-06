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
    public class EstadoCivilConfiguration : EntityTypeConfiguration<EstadoCivil>, IConfiguration
    {
        public EstadoCivilConfiguration()
        {
            ToTable("SPD_ESTADO_CIVIL");

            HasKey(estado => estado.ID, opcoes => opcoes.HasName("SPD_ESTADO_CIVIL_PK"));

            Property(estado => estado.ID)
           .HasColumnName("id_estado")
           .IsRequired();

            Property(estado => estado.DESCRICAO)
           .HasColumnName("descricao")
           .HasMaxLength(50)
           .IsRequired();
        }
    }
}
