using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Application.Clientes.Queries.ListarClientes;

public sealed class ListarClientesHandler
    : IRequestHandler<ListarClientesQuery, Result<List<ClienteListItem>>>
{
    private readonly IListarClientesQuery _query;

    public ListarClientesHandler(IListarClientesQuery query)
    {
        _query = query;
    }

    public async Task<Result<List<ClienteListItem>>> Handle(
        ListarClientesQuery request,
        CancellationToken cancellationToken)
    {
        return await _query.Listar(cancellationToken);
    }
}
