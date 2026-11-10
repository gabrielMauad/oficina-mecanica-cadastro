using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Application.Servicos.Queries.ListarServicos;

public sealed record ListarServicosQuery : IRequest<Result<List<ServicoListItem>>>;

