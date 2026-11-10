using Cadastro.Adapters.DataSources.Records;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cadastro.Infrastructure.Persistence.Configurations;

internal sealed class ClienteConfiguration : IEntityTypeConfiguration<ClienteRecord>
{
    public void Configure(EntityTypeBuilder<ClienteRecord> builder)
    {
        builder.ToTable("cliente", t =>
            t.HasCheckConstraint("CK_cliente_documento_digits", "documento ~ '^[0-9]+$'"));


        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id");

        builder.Property(c => c.Nome)
            .HasColumnName("nome")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.Documento)
            .HasColumnName("documento")
            .HasMaxLength(14)
            .IsRequired();

        builder.HasIndex(c => c.Documento).IsUnique();

        builder.Property(c => c.Email)
            .HasColumnName("email")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.Telefone)
            .HasColumnName("telefone")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.Ativo)
            .HasColumnName("ativo")
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(c => c.CadastradoEm)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(c => c.AtualizadoEm)
            .HasColumnName("updated_at")
            .IsRequired();
    }
}
