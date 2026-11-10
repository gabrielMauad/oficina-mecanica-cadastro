using FluentValidation;

namespace Cadastro.Application.Servicos.Commands.DesativarServico;

public sealed class DesativarServicoValidator : AbstractValidator<DesativarServicoCommand>
{
    public DesativarServicoValidator()
    {
        RuleFor(x => x.ServicoId)
            .NotEmpty()
            .WithMessage("Id do serviço é obrigatório.");
    }
}

