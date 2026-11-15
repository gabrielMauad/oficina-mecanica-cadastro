using Cadastro.Application.Clientes.Commands.CadastrarCliente;

namespace Cadastro.Application.Tests.Cliente.Commands;

public class CadastrarClienteValidatorTests
{
    private readonly CadastrarClienteValidator _validator = new();

    private static CadastrarClienteCommand Comando(string? nome = null, string? email = null) =>
        new(nome ?? "Maria Souza", "12345678909", email ?? "maria@exemplo.com", "(11) 91234-5678", true);

    [Fact(DisplayName = "E-mail com 200 caracteres (limite da coluna) é válido")]
    public void Validate_ShouldBeValid_WhenEmailHasExactlyColumnLimit()
    {
        var email = new string('a', 194) + "@x.com"; // 200

        Assert.Equal(200, email.Length);
        Assert.True(_validator.Validate(Comando(email: email)).IsValid);
    }

    [Fact(DisplayName = "E-mail com mais de 200 caracteres é inválido")]
    public void Validate_ShouldBeInvalid_WhenEmailExceedsColumnLimit()
    {
        var email = new string('a', 195) + "@x.com"; // 201

        var result = _validator.Validate(Comando(email: email));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CadastrarClienteCommand.Email));
    }

    [Fact(DisplayName = "Nome com mais de 200 caracteres é inválido")]
    public void Validate_ShouldBeInvalid_WhenNomeExceedsColumnLimit()
    {
        var result = _validator.Validate(Comando(nome: new string('a', 201)));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CadastrarClienteCommand.Nome));
    }
}
