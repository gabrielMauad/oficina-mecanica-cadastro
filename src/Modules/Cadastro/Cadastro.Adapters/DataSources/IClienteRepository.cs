using Cadastro.Adapters.DataSources.Records;

namespace Cadastro.Adapters.DataSources;

public interface IClienteRepository
{
    Task Adicionar(ClienteRecord cliente, CancellationToken ct = default);
    Task<ClienteRecord?> ObterPorId(Guid id, CancellationToken ct = default);
    Task<ClienteRecord?> ObterPorDocumento(string documento, CancellationToken ct = default);
    Task<bool> ExistePorDocumento(string documento, CancellationToken ct = default);
    Task Atualizar(ClienteRecord cliente, CancellationToken ct = default);
}
