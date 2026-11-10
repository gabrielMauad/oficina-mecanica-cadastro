namespace Cadastro.Adapters.Models.ViewModels;

public sealed record CadastrarClienteViewModel(
    Guid ClienteId,
    string Nome,
    string Documento,
    string Email,
    string Telefone,
    bool Ativo,
    DateTime CadastradoEm,
    DateTime AtualizadoEm);
