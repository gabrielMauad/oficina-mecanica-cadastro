using Cadastro.Domain.Cliente;
using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Application.Clientes.Queries.ObterClientePorId;

public sealed record ObterClientePorIdQuery(Guid ClienteId)
    : IRequest<Result<Cliente>>;
