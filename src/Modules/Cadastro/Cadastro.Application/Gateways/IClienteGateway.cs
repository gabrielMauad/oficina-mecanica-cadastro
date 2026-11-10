using Cadastro.Domain.Cliente;

namespace Cadastro.Application.Gateways;

public interface IClienteGateway
{
    Task Adicionar(Cliente cliente, CancellationToken ct = default);
    Task<Cliente?> ObterPorId(ClienteId id, CancellationToken ct = default);
    Task<Cliente?> ObterPorDocumento(string documento, CancellationToken ct = default);
    Task<bool> ExistePorDocumento(string documento, CancellationToken ct = default);
    Task Atualizar(Cliente cliente, CancellationToken ct = default);
}
