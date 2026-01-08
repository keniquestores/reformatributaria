using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReformaTributaria.Domain.Entities;

namespace ReformaTributaria.Infra.Mappings
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnType("bigint")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.QuestorId)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(c => c.RazaoSocial)
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder.Property(c => c.InscricaoFederal)
                .HasColumnType("varchar(20)")
                .IsRequired();

            builder.Property(c => c.Ativo)
                .HasColumnType("boolean")
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(c => c.ChavePrivada)
                .HasColumnType("text")
                .IsRequired(false);

            builder.Property(c => c.ClientId)
                .HasColumnType("varchar(255)")
                .IsRequired(false);

            builder.Property(c => c.ClientSecret)
                .HasColumnType("varchar(255)")
                .IsRequired(false);

            builder.HasIndex(c => c.QuestorId)
                .HasDatabaseName("ix_clientes_questor_id")
                .IsUnique();

            builder.HasIndex(c => c.InscricaoFederal)
                .HasDatabaseName("ix_clientes_inscricao_federal")
                .IsUnique();

            builder.Property(c => c.Id).UseIdentityColumn();
        }
    }
}