using DocumentValidator;
using SharedKernel.Domain;

namespace Cadastro.Domain.Cliente;

public sealed class Cpf : Documento
{

    internal Cpf(string numero) : base(numero) { }

    public static Result<Cpf> Criar(string numero)
    {
        if (string.IsNullOrWhiteSpace(numero))
            return Error.Validation("CPF.Invalido", "CPF é obrigatório.");

        var digits = new string(numero.Where(char.IsDigit).ToArray());
        var cpf = new Cpf(digits);
        if (!cpf.ValidarDocumento(digits))
            return Error.Validation("CPF.Invalido", "CPF inválido.");

        return cpf;
    }

    protected override bool ValidarDocumento(string numero)
    {
        return CpfValidation.Validate(numero);
    }
}
