using FluentValidation;

namespace Cadastro.Application.Veiculos.Queries.ObterVeiculoPorId;

public sealed class ObterVeiculoPorIdValidator : AbstractValidator<ObterVeiculoPorIdQuery>
{
    public ObterVeiculoPorIdValidator()
    {
        RuleFor(x => x.VeiculoId)
            .NotEmpty().WithMessage("O Id do veículo é obrigatório.");
    }
}

