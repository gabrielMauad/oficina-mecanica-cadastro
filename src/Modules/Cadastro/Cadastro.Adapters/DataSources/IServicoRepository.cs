using Cadastro.Adapters.DataSources.Records;

namespace Cadastro.Adapters.DataSources;

public interface IServicoRepository
{
    Task Adicionar(ServicoRecord servico, CancellationToken ct = default);
    Task<ServicoRecord?> ObterPorId(Guid id, CancellationToken ct = default);
    Task<bool> ExistePorNome(string nome, CancellationToken ct = default);
    Task Atualizar(ServicoRecord servico, CancellationToken ct = default);
}
