using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Application.Servicos.Queries.ListarServicos;

public sealed class ListarServicosHandler
    : IRequestHandler<ListarServicosQuery, Result<List<ServicoListItem>>>
{
    private readonly IListarServicosQuery _query;
    public ListarServicosHandler(IListarServicosQuery query) => _query = query;

    public async Task<Result<List<ServicoListItem>>> Handle(
        ListarServicosQuery request,
        CancellationToken cancellationToken)
    {
        return await _query.Listar(cancellationToken);
    }
}

