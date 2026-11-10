using Cadastro.Application.Gateways;
using Cadastro.Domain.Cliente;
using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Application.Clientes.Commands.AtualizarCliente;

public sealed class AtualizarClienteHandler
        : IRequestHandler<AtualizarClienteCommand, Result<Cliente>>
{
    private readonly IClienteGateway _gateway;

    public AtualizarClienteHandler(IClienteGateway gateway)
    {
        _gateway = gateway;
    }

    public async Task<Result<Cliente>> Handle(
        AtualizarClienteCommand command,
        CancellationToken cancellationToken
    )
    {
        ClienteId clienteId = new(command.Id);
        Cliente? cliente = await _gateway.ObterPorId(clienteId, cancellationToken);

        if (cliente is null)
            return ClienteErrors.NaoEncontrado;

        if (!cliente.Ativo)
            return ClienteErrors.JaDesativado;

        bool houveAlteracao = false;

        if (StringHasChanges(command.Nome, cliente.Nome))
        {
            var result = cliente.AtualizarNome(command.Nome!);
            if (result.IsFailure) return result.Error;
            houveAlteracao = true;
        }

        var telefoneNormalizado = command.Telefone is null
            ? null
            : new string(command.Telefone.Where(char.IsDigit).ToArray());

        if (StringHasChanges(telefoneNormalizado, cliente.Telefone))
        {
            var result = cliente.AtualizarTelefone(command.Telefone!);
            if (result.IsFailure) return result.Error;
            houveAlteracao = true;
        }

        if (houveAlteracao)
            await _gateway.Atualizar(cliente, cancellationToken);

        return cliente;
    }

    private static bool StringHasChanges(string? newValue, string oldValue)
        => newValue is not null && newValue != oldValue;
}
