namespace Cadastro.Application.Veiculos.Queries.ListarVeiculosPorCliente;

public interface IListarVeiculosPorClienteQuery
{
    Task<VeiculosPorCliente?> ListarPorClienteId(Guid clienteId, CancellationToken ct = default);
}

