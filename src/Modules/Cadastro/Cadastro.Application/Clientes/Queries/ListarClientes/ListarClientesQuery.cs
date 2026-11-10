using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Application.Clientes.Queries.ListarClientes;

public sealed record ListarClientesQuery : IRequest<Result<List<ClienteListItem>>>;
