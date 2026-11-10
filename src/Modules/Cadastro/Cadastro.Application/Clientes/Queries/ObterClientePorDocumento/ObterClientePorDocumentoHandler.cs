using Cadastro.Application.Gateways;
using Cadastro.Domain.Cliente;
using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Application.Clientes.Queries.ObterClientePorDocumento;

public sealed class ObterClientePorDocumentoHandler
    : IRequestHandler<ObterClientePorDocumentoQuery, Result<Cliente>>
{
    private readonly IClienteGateway _gateway;

    public ObterClientePorDocumentoHandler(IClienteGateway gateway)
    {
        _gateway = gateway;
    }

    public async Task<Result<Cliente>> Handle(
        ObterClientePorDocumentoQuery query,
        CancellationToken cancellationToken)
    {
        var cliente = await _gateway.ObterPorDocumento(query.Documento, cancellationToken);

        if (cliente is null)
            return ClienteErrors.NaoEncontrado;

        return cliente;
    }
}
