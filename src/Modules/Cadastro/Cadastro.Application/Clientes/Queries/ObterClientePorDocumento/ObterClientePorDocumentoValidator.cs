using FluentValidation;

namespace Cadastro.Application.Clientes.Queries.ObterClientePorDocumento;

public sealed class ObterClientePorDocumentoValidator : AbstractValidator<ObterClientePorDocumentoQuery>
{
    public ObterClientePorDocumentoValidator()
    {
        RuleFor(x => x.Documento)
            .NotEmpty()
            // Só dígitos ASCII 0-9 (\d aceitaria dígitos Unicode) e \z não aceita "\n" no final.
            .Matches(@"\A[0-9]+\z")
            .WithMessage("Documento inválido. Informe somente dígitos.");
    }
}
