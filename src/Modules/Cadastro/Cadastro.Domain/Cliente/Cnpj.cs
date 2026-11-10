using DocumentValidator;
using SharedKernel.Domain;

namespace Cadastro.Domain.Cliente;

public sealed class Cnpj : Documento
{
    internal Cnpj(string numero) : base(numero) { }

    public static Result<Cnpj> Criar(string numero)
    {
        if (string.IsNullOrWhiteSpace(numero))
            return Error.Validation("CNPJ.Invalido", "CNPJ é obrigatório.");

        var digits = new string(numero.Where(char.IsDigit).ToArray());
        var cnpj = new Cnpj(digits);
        if (!cnpj.ValidarDocumento(digits))
            return Error.Validation("CNPJ.Invalido", "CNPJ inválido.");

        return cnpj;
    }

    protected override bool ValidarDocumento(string numero)
    {
        return CnpjValidation.Validate(numero);
    }
}
