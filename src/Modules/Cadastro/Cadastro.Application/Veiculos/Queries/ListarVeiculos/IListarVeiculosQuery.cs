namespace Cadastro.Application.Veiculos.Queries.ListarVeiculos;

public interface IListarVeiculosQuery
{
    Task<List<VeiculoListItem>> Listar(CancellationToken ct = default);
}

