using FluentValidation;

namespace Cadastro.Application.Clientes.Commands.AtualizarCliente;

public sealed class AtualizarClienteValidator : AbstractValidator<AtualizarClienteCommand>
{
    public AtualizarClienteValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Nome)
            .MaximumLength(200)
            .Matches(@"^[\p{L}\s]+$")
            .WithMessage("Nome inválido. Não são permitidos números ou caracteres especiais.")
            .When(x => x.Nome is not null);

        RuleFor(x => x.Telefone)
            .Matches(@"^[\d\s()\-]+$")
            .WithMessage("Telefone inválido.")
            .Must(t => t is null || System.Text.RegularExpressions.Regex.IsMatch(
                new string(t.Where(char.IsDigit).ToArray()), @"^\d{2}9\d{8}$"))
            .WithMessage("Telefone inválido. Formato aceito: (11) 91234-5678.")
            .When(x => x.Telefone is not null);
    }
}

