namespace Cadastro.Adapters.Models.Request;

public sealed record AdicionarServicoRequest(
    string Nome,
    string? Descricao,
    decimal Preco);
