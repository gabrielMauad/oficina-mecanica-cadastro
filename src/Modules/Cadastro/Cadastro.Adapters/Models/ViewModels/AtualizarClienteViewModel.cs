namespace Cadastro.Adapters.Models.ViewModels;

public sealed record AtualizarClienteViewModel(
    Guid ClienteId,
    string Nome,
    string Telefone,
    bool Ativo,
    DateTime CadastradoEm,
    DateTime AtualizadoEm);
