using FluentValidation;

namespace Cadastro.Application.Clientes.Queries.ObterClientePorDocumento;

public sealed class ObterClientePorDocumentoValidator : AbstractValidator<ObterClientePorDocumentoQuery>
{
    public ObterClientePorDocumentoValidator()
    {
        RuleFor(x => x.Documento)
            .NotEmpty()
            .Matches(@"^\d+$")
            .WithMessage("Documento inválido. Informe somente dígitos.");
    }
}
