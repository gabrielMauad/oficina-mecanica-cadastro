using FluentValidation;

namespace Cadastro.Application.Clientes.Commands.CadastrarCliente;

public sealed class CadastrarClienteValidator : AbstractValidator<CadastrarClienteCommand>
{
    public CadastrarClienteValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .MaximumLength(200)
            .Matches(@"^[\p{L}\s]+$")
            .WithMessage("Nome inválido. Não são permitidos números ou caracteres especiais.");

        RuleFor(x => x.Documento)
            .NotEmpty()
            .Matches(@"^[\d.\-/]+$")
            .WithMessage("Documento inválido.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
            .WithMessage("Email inválido.");

        RuleFor(x => x.Telefone)
            .NotEmpty()
            .Matches(@"^[\d\s()\-]+$")
            .WithMessage("Telefone inválido.")
            .Must(t => t is null || System.Text.RegularExpressions.Regex.IsMatch(
                new string(t.Where(char.IsDigit).ToArray()), @"^\d{2}9\d{8}$"))
            .WithMessage("Telefone inválido. Formato aceito: (11) 91234-5678.");
    }
}
