using Cadastro.Contracts.Dtos;
using Cadastro.Contracts.Queries;
using Cadastro.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Infrastructure.Queries;

internal sealed class CadastroServicoQuery : ICadastroServicoQuery
{
    private readonly CadastroDbContext _context;

    public CadastroServicoQuery(CadastroDbContext context) => _context = context;

    public async Task<ServicoDto?> ObterPorId(Guid id, CancellationToken ct = default)
    {
        var servico = await _context.Servicos
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, ct);

        if (servico is null)
            return null;

        return new ServicoDto(
            servico.Id,
            servico.Nome,
            servico.PrecoBase,
            servico.Ativo);
    }
}
