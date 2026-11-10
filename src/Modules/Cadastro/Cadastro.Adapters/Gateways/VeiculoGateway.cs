using Cadastro.Adapters.DataSources;
using Cadastro.Adapters.DataSources.Mappers;
using Cadastro.Application.Gateways;
using Cadastro.Domain.Veiculo;
using SharedKernel.Application;

namespace Cadastro.Adapters.Gateways;

public sealed class VeiculoGateway : IVeiculoGateway
{
    private readonly IVeiculoRepository _repository;
    private readonly IDomainEventCollector _collector;

    public VeiculoGateway(IVeiculoRepository repository, IDomainEventCollector collector)
    {
        _repository = repository;
        _collector = collector;
    }

    public Task Adicionar(Veiculo veiculo, CancellationToken ct = default)
    {
        _collector.Registrar(veiculo);
        return _repository.Adicionar(VeiculoMapper.ToRecord(veiculo), ct);
    }

    public async Task<Veiculo?> ObterPorId(VeiculoId id, CancellationToken ct = default)
    {
        var record = await _repository.ObterPorId(id.Value, ct);
        return record is null ? null : VeiculoMapper.ToDomain(record);
    }

    public Task<bool> ExistePorPlaca(string placa, CancellationToken ct = default) =>
        _repository.ExistePorPlaca(placa, ct);
}
