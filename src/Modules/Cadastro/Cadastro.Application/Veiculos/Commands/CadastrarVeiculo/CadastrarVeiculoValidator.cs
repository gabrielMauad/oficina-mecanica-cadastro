using FluentValidation;

namespace Cadastro.Application.Veiculos.Commands.CadastrarVeiculo;

public sealed class CadastrarVeiculoValidator : AbstractValidator<CadastrarVeiculoCommand>
{
    public CadastrarVeiculoValidator()
    {
        RuleFor(x => x.Placa)
            .NotEmpty()
            .MaximumLength(8)
            .Matches(@"^[A-Za-z0-9\-]+$")
            .WithMessage("Placa inválida.");

        RuleFor(x => x.Modelo)
            .NotEmpty()
            .WithMessage("Modelo é obrigatório.")
            .MaximumLength(100)
            .WithMessage("Modelo deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Marca)
            .NotEmpty()
            .WithMessage("Marca é obrigatória.")
            .MaximumLength(100)
            .WithMessage("Marca deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Ano)
            .InclusiveBetween(1886, DateTime.UtcNow.Year + 1)
            .WithMessage("Ano deve ser um ano válido.");

        RuleFor(x => x.ClienteId)
            .NotEmpty()
            .WithMessage("Id do cliente é obrigatório.");
    }
}

