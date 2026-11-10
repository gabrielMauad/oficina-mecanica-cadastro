using Cadastro.Adapters.DataSources.Records;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cadastro.Infrastructure.Persistence.Configurations;

internal sealed class VeiculoConfiguration : IEntityTypeConfiguration<VeiculoRecord>
{
    public void Configure(EntityTypeBuilder<VeiculoRecord> builder)
    {
        builder.ToTable("veiculo");

        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id)
            .HasColumnName("id");

        builder.Property(v => v.Placa)
            .HasColumnName("placa")
            .HasMaxLength(8)
            .IsRequired();

        builder.HasIndex(v => v.Placa).IsUnique();

        builder.Property(v => v.Modelo)
            .HasColumnName("modelo")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(v => v.Marca)
            .HasColumnName("marca")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(v => v.Ano)
            .HasColumnName("ano")
            .IsRequired();

        builder.Property(v => v.ClienteId)
            .HasColumnName("cliente_id")
            .IsRequired();

        builder.HasOne<ClienteRecord>()
            .WithMany()
            .HasForeignKey(v => v.ClienteId)
            .IsRequired();

        builder.Property(v => v.CadastradoEm)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(v => v.AtualizadoEm)
            .HasColumnName("updated_at")
            .IsRequired();
    }
}
