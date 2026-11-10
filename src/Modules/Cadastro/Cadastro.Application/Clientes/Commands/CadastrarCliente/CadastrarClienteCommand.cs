using Cadastro.Domain.Cliente;
using SharedKernel.Application;

namespace Cadastro.Application.Clientes.Commands.CadastrarCliente;

public sealed record CadastrarClienteCommand(
    string Nome,
    string Documento,
    string Email,
    string Telefone,
    bool PessoaFisica
) : ICommand<Cliente>;
