using Cadastro.Adapters.DataSources;
using Cadastro.Adapters.DataSources.Records;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Infrastructure.Persistence.Repositories;

internal sealed class ServicoRepository : IServicoRepository
{
    private readonly CadastroDbContext _context;

    public ServicoRepository(CadastroDbContext context) => _context = context;

    public Task Adicionar(ServicoRecord servico, CancellationToken ct = default)
    {
        _context.Servicos.Add(servico);
        return Task.CompletedTask;
    }

    public Task<ServicoRecord?> ObterPorId(Guid id, CancellationToken ct = default) =>
        _context.Servicos.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, ct);

    public Task<bool> ExistePorNome(string nome, CancellationToken ct = default) =>
        _context.Servicos.AnyAsync(s => s.Nome == nome, ct);

    public Task Atualizar(ServicoRecord servico, CancellationToken ct = default)
    {
        _context.Servicos.Update(servico);
        return Task.CompletedTask;
    }
}
