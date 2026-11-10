using Cadastro.Domain.Cliente;
using SharedKernel.Application;

namespace Cadastro.Application.Clientes.Commands.DesativarCliente;

public sealed record DesativarClienteCommand(Guid ClienteId) : ICommand<Cliente>;