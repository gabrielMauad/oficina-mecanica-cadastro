using Cadastro.Adapters.DataSources.Records;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cadastro.Infrastructure.Persistence.Configurations;

internal sealed class ServicoConfiguration : IEntityTypeConfiguration<ServicoRecord>
{
    public void Configure(EntityTypeBuilder<ServicoRecord> builder)
    {
        builder.ToTable("servico");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasColumnName("id");

        builder.Property(s => s.Nome)
            .HasColumnName("nome")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(s => s.Descricao)
            .HasColumnName("descricao");

        builder.Property(s => s.PrecoBase)
            .HasColumnName("preco_base")
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(s => s.Ativo)
            .HasColumnName("ativo")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(s => s.CadastradoEm)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(s => s.AtualizadoEm)
            .HasColumnName("updated_at")
            .IsRequired();
    }
}
