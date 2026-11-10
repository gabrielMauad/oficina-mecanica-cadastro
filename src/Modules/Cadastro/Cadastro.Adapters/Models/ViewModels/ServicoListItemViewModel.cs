namespace Cadastro.Adapters.Models.ViewModels;

public sealed record ServicoListItemViewModel(
    Guid Id,
    string Nome,
    string? Descricao,
    decimal Preco,
    bool Ativo,
    DateTime CadastradoEm,
    DateTime AtualizadoEm);
