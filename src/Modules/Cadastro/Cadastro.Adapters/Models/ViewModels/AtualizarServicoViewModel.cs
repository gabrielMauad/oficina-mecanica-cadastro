namespace Cadastro.Adapters.Models.ViewModels;

public sealed record AtualizarServicoViewModel(
    Guid ServicoId,
    string? Descricao,
    decimal Preco,
    bool Ativo,
    DateTime CadastradoEm,
    DateTime AtualizadoEm);
