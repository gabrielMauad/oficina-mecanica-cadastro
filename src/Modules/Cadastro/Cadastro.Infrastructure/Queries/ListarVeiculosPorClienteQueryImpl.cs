using Cadastro.Application.Veiculos.Queries.ListarVeiculosPorCliente;
using Cadastro.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Infrastructure.Queries;

internal sealed class ListarVeiculosPorClienteQueryImpl : IListarVeiculosPorClienteQuery
{
    private readonly CadastroDbContext _context;

    public ListarVeiculosPorClienteQueryImpl(CadastroDbContext context) => _context = context;

    public Task<VeiculosPorCliente?> ListarPorClienteId(Guid clienteId, CancellationToken ct = default)
    {
        return _context.Clientes
            .AsNoTracking()
            .Where(c => c.Id == clienteId)
            .Select(c => new VeiculosPorCliente(
                c.Id,
                c.Nome,
                _context.Veiculos
                    .Where(v => v.ClienteId == clienteId)
                    .Select(v => new VeiculoDoCliente(
                        v.Id,
                        v.Placa,
                        v.Modelo,
                        v.Marca,
                        v.Ano,
                        v.CadastradoEm,
                        v.AtualizadoEm))
                    .ToList()))
            .FirstOrDefaultAsync(ct);
    }
}
