using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Application.Veiculos.Queries.ListarVeiculos;

public sealed record ListarVeiculosQuery : IRequest<Result<List<VeiculoListItem>>>;

