using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Application.Veiculos.Queries.ListarVeiculosPorCliente;

public sealed record ListarVeiculosPorClienteQuery(Guid ClienteId) : IRequest<Result<VeiculosPorCliente>>;
