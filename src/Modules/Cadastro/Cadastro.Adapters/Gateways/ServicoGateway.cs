using Cadastro.Adapters.DataSources;
using Cadastro.Adapters.DataSources.Mappers;
using Cadastro.Application.Gateways;
using Cadastro.Domain.Servico;
using SharedKernel.Application;

namespace Cadastro.Adapters.Gateways;

public sealed class ServicoGateway : IServicoGateway
{
    private readonly IServicoRepository _repository;
    private readonly IDomainEventCollector _collector;

    public ServicoGateway(IServicoRepository repository, IDomainEventCollector collector)
    {
        _repository = repository;
        _collector = collector;
    }

    public Task Adicionar(Servico servico, CancellationToken ct = default)
    {
        _collector.Registrar(servico);
        return _repository.Adicionar(ServicoMapper.ToRecord(servico), ct);
    }

    public async Task<Servico?> ObterPorId(ServicoId id, CancellationToken ct = default)
    {
        var record = await _repository.ObterPorId(id.Value, ct);
        return record is null ? null : ServicoMapper.ToDomain(record);
    }

    public Task<bool> ExistePorNome(string nome, CancellationToken ct = default) =>
        _repository.ExistePorNome(nome, ct);

    public Task Atualizar(Servico servico, CancellationToken ct = default)
    {
        _collector.Registrar(servico);
        return _repository.Atualizar(ServicoMapper.ToRecord(servico), ct);
    }
}
