using Cadastro.Contracts.Dtos;

namespace Cadastro.Contracts.Queries;

public interface ICadastroVeiculoQuery
{
    Task<VeiculoDto?> ObterPorId(Guid id, CancellationToken ct = default);
}

