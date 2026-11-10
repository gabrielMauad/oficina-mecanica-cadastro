namespace Cadastro.Adapters.Models.ViewModels;

public sealed record AdicionarServicoViewModel(
    Guid ServicoId,
    string Nome,
    string? Descricao,
    decimal PrecoBase,
    bool Ativo,
    DateTime CadastradoEm,
    DateTime AtualizadoEm);
