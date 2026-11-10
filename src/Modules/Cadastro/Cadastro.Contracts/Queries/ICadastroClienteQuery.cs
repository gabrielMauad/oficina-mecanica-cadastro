using Cadastro.Contracts.Dtos;

namespace Cadastro.Contracts.Queries;
public interface ICadastroClienteQuery
{
    Task<ClienteDto?> ObterPorId(Guid id, CancellationToken ct = default);
}

