using SharedKernel.Domain;

namespace Cadastro.Domain.Cliente;

public abstract class Documento : ValueObject
{
    public string Numero { get; }

    protected Documento(string numero) => Numero = numero;

    protected override IEnumerable<object?> GetComponents()
    {
        yield return Numero;
    }

    protected abstract bool ValidarDocumento(string numero);

    public static Documento Reconstituir(string numero) =>
        numero.Length <= 11 ? new Cpf(numero) : new Cnpj(numero);
}

