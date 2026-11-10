using Cadastro.Adapters.DataSources;
using Cadastro.Adapters.DataSources.Records;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Infrastructure.Persistence.Repositories;

internal sealed class VeiculoRepository : IVeiculoRepository
{
    private readonly CadastroDbContext _context;

    public VeiculoRepository(CadastroDbContext context) => _context = context;

    public Task Adicionar(VeiculoRecord veiculo, CancellationToken ct = default)
    {
        _context.Veiculos.Add(veiculo);
        return Task.CompletedTask;
    }

    public Task<VeiculoRecord?> ObterPorId(Guid id, CancellationToken ct = default) =>
        _context.Veiculos.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id, ct);

    public Task<bool> ExistePorPlaca(string placa, CancellationToken ct = default) =>
        _context.Database
            .SqlQuery<bool>($"SELECT EXISTS(SELECT 1 FROM cadastro.veiculo WHERE placa = {placa}) AS \"Value\"")
            .FirstAsync(ct);
}
