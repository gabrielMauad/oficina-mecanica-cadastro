using FluentValidation;

namespace Cadastro.Application.Clientes.Queries.ObterClientePorId;

public sealed class ObterClientePorIdValidator : AbstractValidator<ObterClientePorIdQuery>
{
    public ObterClientePorIdValidator()
    {
        RuleFor(x => x.ClienteId)
            .NotEmpty();
    }
}
