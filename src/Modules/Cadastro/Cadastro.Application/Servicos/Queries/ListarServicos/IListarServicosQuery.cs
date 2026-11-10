namespace Cadastro.Application.Servicos.Queries.ListarServicos;

public interface IListarServicosQuery
{
    Task<List<ServicoListItem>> Listar(CancellationToken ct = default);
}

