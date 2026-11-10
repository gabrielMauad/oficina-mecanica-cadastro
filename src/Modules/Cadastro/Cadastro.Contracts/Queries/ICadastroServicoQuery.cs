using Cadastro.Contracts.Dtos;

namespace Cadastro.Contracts.Queries;

public interface ICadastroServicoQuery
{
    Task<ServicoDto?> ObterPorId(Guid id, CancellationToken ct = default);
}

