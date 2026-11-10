using FluentValidation;

namespace Cadastro.Application.Veiculos.Queries.ListarVeiculosPorCliente;

public sealed class ListarVeiculosPorClienteValidator : AbstractValidator<ListarVeiculosPorClienteQuery>
{
    public ListarVeiculosPorClienteValidator()
    {
        RuleFor(x => x.ClienteId)
            .NotEmpty()
            .WithMessage("O Id do cliente é obrigatório.");
    }
}
