using Cadastro.Application.Gateways;
using Cadastro.Domain.Cliente;
using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Application.Clientes.Commands.CadastrarCliente;

public sealed class CadastrarClienteHandler
    : IRequestHandler<CadastrarClienteCommand, Result<Cliente>>
{
    private readonly IClienteGateway _gateway;

    public CadastrarClienteHandler(IClienteGateway gateway)
    {
        _gateway = gateway;
    }

    public async Task<Result<Cliente>> Handle(
        CadastrarClienteCommand command,
        CancellationToken cancellationToken)
    {
        var documentoNormalizado = new string(command.Documento.Where(char.IsDigit).ToArray());

        if (await _gateway.ExistePorDocumento(documentoNormalizado, cancellationToken))
            return ClienteErrors.DocumentoJaExiste;

        var result = Cliente.Criar(
            command.Nome,
            command.Documento,
            command.Email,
            command.Telefone,
            command.PessoaFisica);

        if (result.IsFailure)
            return result.Error;

        var cliente = result.Value;

        await _gateway.Adicionar(cliente, cancellationToken);

        return cliente;
    }
}
