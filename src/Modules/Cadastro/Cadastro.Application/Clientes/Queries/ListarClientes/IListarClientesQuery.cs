namespace Cadastro.Application.Clientes.Queries.ListarClientes;

public interface IListarClientesQuery
{
    Task<List<ClienteListItem>> Listar(CancellationToken ct = default);
}
