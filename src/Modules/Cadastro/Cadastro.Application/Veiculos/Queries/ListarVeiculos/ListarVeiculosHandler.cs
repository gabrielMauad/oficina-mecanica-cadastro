using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Application.Veiculos.Queries.ListarVeiculos;

public sealed class ListarVeiculosHandler
    : IRequestHandler<ListarVeiculosQuery, Result<List<VeiculoListItem>>>
{
    private readonly IListarVeiculosQuery _query;

    public ListarVeiculosHandler(IListarVeiculosQuery query)
    {
        _query = query;
    }

    public async Task<Result<List<VeiculoListItem>>> Handle(
        ListarVeiculosQuery request,
        CancellationToken cancellationToken)
    {
        return await _query.Listar(cancellationToken);
    }
}

