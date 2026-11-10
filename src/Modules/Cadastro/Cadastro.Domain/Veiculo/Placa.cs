using System.Text.RegularExpressions;
using SharedKernel.Domain;

namespace Cadastro.Domain.Veiculo;

public sealed class Placa : ValueObject
{
    // Formato antigo: ABC1234 | Mercosul: ABC1D23
    private static readonly Regex FormatoValido = new(@"^[A-Z]{3}\d{4}$|^[A-Z]{3}\d[A-Z]\d{2}$", RegexOptions.Compiled);

    public string Numero { get; }

    private Placa(string numero) => Numero = numero;

    public static Placa Reconstituir(string numero) => new(numero);

    public static Result<Placa> Criar(string numero)
    {
        if (string.IsNullOrWhiteSpace(numero))
            return Error.Validation("Placa.Invalida", "Placa é obrigatória.");

        var normalizado = numero.ToUpperInvariant().Replace("-", "");

        if (!FormatoValido.IsMatch(normalizado))
            return Error.Validation("Placa.Invalida", "Placa inválida.");

        return new Placa(normalizado);
    }

    protected override IEnumerable<object?> GetComponents()
    {
        yield return Numero;
    }
}

