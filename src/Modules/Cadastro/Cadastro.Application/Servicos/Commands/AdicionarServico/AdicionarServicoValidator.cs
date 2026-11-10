using FluentValidation;

namespace Cadastro.Application.Servicos.Commands.AdicionarServico;

public sealed class AdicionarServicoValidator : AbstractValidator<AdicionarServicoCommand>
{
    public AdicionarServicoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("O nome do serviço é obrigatório.")
            .MaximumLength(200)
            .WithMessage("O nome do serviço deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Descricao)
            .MaximumLength(500)
            .WithMessage("A descrição do serviço deve ter no máximo 500 caracteres.")
            .When(x => x.Descricao is not null);

        RuleFor(x => x.Preco)
            .GreaterThanOrEqualTo(0)
            .WithMessage("O preço do serviço não deve ser negativo.");
    }
}