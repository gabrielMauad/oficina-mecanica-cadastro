using Cadastro.Application.Servicos.Queries.ListarServicos;
using Cadastro.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Infrastructure.Queries;

internal sealed class ListarServicosQueryImpl : IListarServicosQuery
{
    private readonly CadastroDbContext _context;

    public ListarServicosQueryImpl(CadastroDbContext context) => _context = context;

    public async Task<List<ServicoListItem>> Listar(CancellationToken ct = default)
    {
        return await _context.Servicos
            .AsNoTracking()
            .Select(s => new ServicoListItem(
                s.Id,
                s.Nome,
                s.Descricao,
                s.PrecoBase,
                s.Ativo,
                s.CadastradoEm,
                s.AtualizadoEm))
            .ToListAsync(ct);
    }
}
