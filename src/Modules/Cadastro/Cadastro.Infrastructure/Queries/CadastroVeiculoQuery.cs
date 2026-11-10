using Cadastro.Contracts.Dtos;
using Cadastro.Contracts.Queries;
using Cadastro.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Infrastructure.Queries;

internal sealed class CadastroVeiculoQuery : ICadastroVeiculoQuery
{
    private readonly CadastroDbContext _context;

    public CadastroVeiculoQuery(CadastroDbContext context) => _context = context;

    public async Task<VeiculoDto?> ObterPorId(Guid id, CancellationToken ct = default)
    {
        var veiculo = await _context.Veiculos
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == id, ct);

        if (veiculo is null)
            return null;

        return new VeiculoDto(
            veiculo.Id,
            veiculo.Placa,
            veiculo.Modelo,
            veiculo.Marca,
            veiculo.Ano,
            veiculo.ClienteId);
    }
}
