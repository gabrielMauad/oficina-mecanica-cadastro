using FluentValidation;

namespace Cadastro.Application.Servicos.Commands.AtualizarServico;

public sealed class AtualizarServicoValidator : AbstractValidator<AtualizarServicoCommand>
{
    public AtualizarServicoValidator()
    {
        RuleFor(x => x.ServicoId)
            .NotEmpty()
            .WithMessage("Id do serviço é obrigatório.");

        RuleFor(x => x.Descricao)
            .MaximumLength(500)
            .WithMessage("Descrição do serviço deve ter no máximo 500 caracteres.")
            .When(x => x.Descricao is not null);

        RuleFor(x => x.Preco)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Preço deve ser maior ou igual a zero.")
            .When(x => x.Preco is not null);
    }
}

