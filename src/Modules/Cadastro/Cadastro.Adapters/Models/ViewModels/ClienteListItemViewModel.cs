namespace Cadastro.Adapters.Models.ViewModels;

public sealed record ClienteListItemViewModel(
    Guid Id,
    string Nome,
    string Documento,
    string Email,
    string Telefone,
    bool Ativo,
    DateTime CadastradoEm,
    DateTime AtualizadoEm);
