using Cadastro.Application.Clientes.Queries.ListarClientes;
using Cadastro.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Infrastructure.Queries;

internal sealed class ListarClientesQueryImpl : IListarClientesQuery
{
    private readonly CadastroDbContext _context;

    public ListarClientesQueryImpl(CadastroDbContext context) => _context = context;

    public Task<List<ClienteListItem>> Listar(CancellationToken ct = default) =>
        _context.Clientes
            .AsNoTracking()
            .Select(c => new ClienteListItem(
                c.Id,
                c.Nome,
                c.Documento,
                c.Email,
                c.Telefone,
                c.Ativo,
                c.CadastradoEm,
                c.AtualizadoEm))
            .ToListAsync(ct);
}
