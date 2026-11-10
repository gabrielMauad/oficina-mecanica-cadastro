using FluentValidation;

namespace Cadastro.Application.Clientes.Commands.DesativarCliente;

public sealed class DesativarClienteValidator : AbstractValidator<DesativarClienteCommand>
{
    public DesativarClienteValidator()
    {
        RuleFor(x => x.ClienteId)
            .NotEmpty();
    }
}
