namespace Cadastro.Adapters.Models.ViewModels;

public sealed record DesativarClienteViewModel(
    Guid ClienteId,
    string Nome,
    bool Ativo,
    DateTime CadastradoEm,
    DateTime AtualizadoEm);
