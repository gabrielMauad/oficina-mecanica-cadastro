using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Cadastro.Infrastructure.Persistence;

internal sealed class CadastroDbContextFactory : IDesignTimeDbContextFactory<CadastroDbContext>
{
    public CadastroDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Default");

        var options = new DbContextOptionsBuilder<CadastroDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new CadastroDbContext(options);
    }
}
