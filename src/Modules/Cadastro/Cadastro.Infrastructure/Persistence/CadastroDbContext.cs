using Cadastro.Adapters.DataSources.Records;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Application;

namespace Cadastro.Infrastructure.Persistence;

public sealed class CadastroDbContext : DbContext, IUnitOfWork
{
    public CadastroDbContext(DbContextOptions<CadastroDbContext> options) : base(options) { }

    public DbSet<ClienteRecord> Clientes => Set<ClienteRecord>();
    public DbSet<VeiculoRecord> Veiculos => Set<VeiculoRecord>();
    public DbSet<ServicoRecord> Servicos => Set<ServicoRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("cadastro");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CadastroDbContext).Assembly);
    }
}
