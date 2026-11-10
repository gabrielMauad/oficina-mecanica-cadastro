using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Application.Veiculos.Queries.ListarVeiculosPorCliente;

public sealed class ListarVeiculosPorClienteHandler
    : IRequestHandler<ListarVeiculosPorClienteQuery, Result<VeiculosPorCliente>>
{
    private readonly IListarVeiculosPorClienteQuery _query;

    public ListarVeiculosPorClienteHandler(IListarVeiculosPorClienteQuery query) => _query = query;

    public async Task<Result<VeiculosPorCliente>> Handle(
        ListarVeiculosPorClienteQuery request,
        CancellationToken cancellationToken)
    {
        var resultado = await _query.ListarPorClienteId(request.ClienteId, cancellationToken);
        if (resultado is null)
            return VeiculoErrors.ClienteNaoEncontrado;
        return resultado;
    }
}