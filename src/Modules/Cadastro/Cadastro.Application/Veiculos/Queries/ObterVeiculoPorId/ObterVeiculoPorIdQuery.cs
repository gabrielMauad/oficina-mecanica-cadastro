using Cadastro.Domain.Veiculo;
using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Application.Veiculos.Queries.ObterVeiculoPorId;

public sealed record ObterVeiculoPorIdQuery(Guid VeiculoId) : IRequest<Result<Veiculo>>;
