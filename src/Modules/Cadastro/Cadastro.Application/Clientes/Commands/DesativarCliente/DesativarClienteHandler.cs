using Cadastro.Application.Gateways;
using Cadastro.Domain.Cliente;
using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Application.Clientes.Commands.DesativarCliente;

public sealed class DesativarClienteHandler : IRequestHandler<DesativarClienteCommand, Result<Cliente>>
{
    private readonly IClienteGateway _gateway;

    public DesativarClienteHandler(IClienteGateway gateway) => _gateway = gateway;

    public async Task<Result<Cliente>> Handle(DesativarClienteCommand command, CancellationToken cancellationToken)
    {
        ClienteId clienteId = new ClienteId(command.ClienteId);
        Cliente? cliente = await _gateway.ObterPorId(clienteId, cancellationToken);
        if (cliente is null)
            return ClienteErrors.NaoEncontrado;
        if (!cliente.Ativo)
            return ClienteErrors.JaDesativado;
        cliente.Desativar();
        await _gateway.Atualizar(cliente, cancellationToken);

        return cliente;
    }

}

