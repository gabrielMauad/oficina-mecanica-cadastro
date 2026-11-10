namespace Cadastro.Adapters.Models.ViewModels;

public sealed record DesativarServicoViewModel(
    Guid ServicoId,
    string Nome,
    bool Ativo);
