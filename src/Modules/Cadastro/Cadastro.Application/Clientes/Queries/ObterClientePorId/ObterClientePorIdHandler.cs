using Cadastro.Application.Gateways;
using Cadastro.Domain.Cliente;
using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Application.Clientes.Queries.ObterClientePorId;

public sealed class ObterClientePorIdHandler
    : IRequestHandler<ObterClientePorIdQuery, Result<Cliente>>
{
    private readonly IClienteGateway _gateway;

    public ObterClientePorIdHandler(IClienteGateway gateway)
    {
        _gateway = gateway;
    }

    public async Task<Result<Cliente>> Handle(
        ObterClientePorIdQuery query,
        CancellationToken cancellationToken)
    {
        var cliente = await _gateway.ObterPorId(
            new ClienteId(query.ClienteId),
            cancellationToken);

        if (cliente is null)
            return ClienteErrors.NaoEncontrado;

        return cliente;
    }
}
