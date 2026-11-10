namespace Cadastro.Application.Servicos.Queries.ListarServicos;

public sealed record ServicoListItem(
    Guid Id,
    string Nome,
    string? Descricao,
    decimal Preco,
    bool Ativo,
    DateTime CadastradoEm,
    DateTime AtualizadoEm
);
