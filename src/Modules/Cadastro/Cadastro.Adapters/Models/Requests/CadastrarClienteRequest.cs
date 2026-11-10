namespace Cadastro.Adapters.Models.Request;

public sealed record CadastrarClienteRequest(
    string Nome,
    string Documento,
    string Email,
    string Telefone,
    bool PessoaFisica);
