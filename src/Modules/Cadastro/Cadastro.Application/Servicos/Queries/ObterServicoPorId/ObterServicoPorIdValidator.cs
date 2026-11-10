using FluentValidation;

namespace Cadastro.Application.Servicos.Queries.ObterServicoPorId;

public sealed class ObterServicoPorIdValidator : AbstractValidator<ObterServicoPorIdQuery>
{
    public ObterServicoPorIdValidator()
    {
        RuleFor(x => x.ServicoId)
            .NotEmpty()
            .WithMessage("O Id do serviço é obrigatório.");
    }
}

