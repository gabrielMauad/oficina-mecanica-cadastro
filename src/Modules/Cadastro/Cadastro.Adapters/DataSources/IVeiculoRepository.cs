using Cadastro.Adapters.DataSources.Records;

namespace Cadastro.Adapters.DataSources;

public interface IVeiculoRepository
{
    Task Adicionar(VeiculoRecord veiculo, CancellationToken ct = default);
    Task<VeiculoRecord?> ObterPorId(Guid id, CancellationToken ct = default);
    Task<bool> ExistePorPlaca(string placa, CancellationToken ct = default);
}
