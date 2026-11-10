using Cadastro.Adapters.DataSources;
using Cadastro.Adapters.DataSources.Mappers;
using Cadastro.Application.Gateways;
using Cadastro.Domain.Cliente;
using SharedKernel.Application;

namespace Cadastro.Adapters.Gateways;

public sealed class ClienteGateway : IClienteGateway
{
    private readonly IClienteRepository _repository;
    private readonly IDomainEventCollector _collector;

    public ClienteGateway(IClienteRepository repository, IDomainEventCollector collector)
    {
        _repository = repository;
        _collector = collector;
    }

    public Task Adicionar(Cliente cliente, CancellationToken ct = default)
    {
        _collector.Registrar(cliente);
        return _repository.Adicionar(ClienteMapper.ToRecord(cliente), ct);
    }

    public async Task<Cliente?> ObterPorId(ClienteId id, CancellationToken ct = default)
    {
        var record = await _repository.ObterPorId(id.Value, ct);
        return record is null ? null : ClienteMapper.ToDomain(record);
    }

    public async Task<Cliente?> ObterPorDocumento(string documento, CancellationToken ct = default)
    {
        var record = await _repository.ObterPorDocumento(documento, ct);
        return record is null ? null : ClienteMapper.ToDomain(record);
    }

    public Task<bool> ExistePorDocumento(string documento, CancellationToken ct = default) =>
        _repository.ExistePorDocumento(documento, ct);

    public Task Atualizar(Cliente cliente, CancellationToken ct = default)
    {
        _collector.Registrar(cliente);
        return _repository.Atualizar(ClienteMapper.ToRecord(cliente), ct);
    }
}
