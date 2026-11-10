using Cadastro.Domain.Veiculo;

namespace Cadastro.Application.Gateways;

public interface IVeiculoGateway
{
    Task Adicionar(Veiculo veiculo, CancellationToken ct = default);
    Task<Veiculo?> ObterPorId(VeiculoId id, CancellationToken ct = default);
    Task<bool> ExistePorPlaca(string placa, CancellationToken ct = default);
}
