using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReformaTributaria.Domain.Entities;

namespace ReformaTributaria.Infra.Mappings
{
    public class FilaConfiguration : IEntityTypeConfiguration<Fila>
    {
        public void Configure(EntityTypeBuilder<Fila> builder)
        {
            builder.ToTable("Fila", "public");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Id)
                .HasColumnType("bigint")
                .ValueGeneratedOnAdd();

            builder.Property(f => f.InscricaoFederalContribuinte)
                .HasColumnType("varchar(20)")
                .IsRequired();

            builder.Property(f => f.Ativo)
                .HasColumnType("boolean")
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasIndex(f => f.InscricaoFederalContribuinte)
                .HasDatabaseName("ix_filas_inscricao_federal_contribuinte")
                .IsUnique();

            builder.HasIndex(f => f.Ativo)
                .HasDatabaseName("ix_filas_ativo");

            builder.Property(f => f.Id).UseIdentityColumn();
        }
    }
}


