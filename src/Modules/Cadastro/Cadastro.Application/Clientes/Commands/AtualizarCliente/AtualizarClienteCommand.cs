using Cadastro.Domain.Cliente;
using SharedKernel.Application;

namespace Cadastro.Application.Clientes.Commands.AtualizarCliente;

public sealed record AtualizarClienteCommand(
    Guid Id,
    string? Nome,
    string? Telefone
) : ICommand<Cliente>;

