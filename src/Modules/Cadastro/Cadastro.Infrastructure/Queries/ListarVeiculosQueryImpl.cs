using Cadastro.Application.Veiculos.Queries.ListarVeiculos;
using Cadastro.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Infrastructure.Queries;

internal sealed class ListarVeiculosQueryImpl : IListarVeiculosQuery
{
    private readonly CadastroDbContext _context;

    public ListarVeiculosQueryImpl(CadastroDbContext context) => _context = context;

    public async Task<List<VeiculoListItem>> Listar(CancellationToken ct = default)
    {
        return await _context.Veiculos
            .AsNoTracking()
            .Select(v => new VeiculoListItem(
                v.Id,
                v.Placa,
                v.Modelo,
                v.Marca,
                v.Ano,
                v.ClienteId,
                v.CadastradoEm,
                v.AtualizadoEm))
            .ToListAsync(ct);
    }
}
