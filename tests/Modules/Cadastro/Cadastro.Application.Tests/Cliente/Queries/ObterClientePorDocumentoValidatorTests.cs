using Cadastro.Application.Clientes.Queries.ObterClientePorDocumento;

namespace Cadastro.Application.Tests.Cliente.Queries;

public class ObterClientePorDocumentoValidatorTests
{
    private readonly ObterClientePorDocumentoValidator _validator = new();

    [Theory(DisplayName = "Documento somente com dígitos é válido")]
    [InlineData("01404238000")]
    [InlineData("12345678909")]
    [InlineData("11222333000181")]
    public void Validate_ShouldBeValid_WhenDocumentoHasOnlyDigits(string documento)
    {
        var result = _validator.Validate(new ObterClientePorDocumentoQuery(documento));

        Assert.True(result.IsValid);
    }

    [Theory(DisplayName = "Documento vazio ou com caracteres não numéricos é inválido")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("123.456.789-09")]
    [InlineData("11.222.333/0001-81")]
    [InlineData("abc12345678")]
    [InlineData("12345678909 ")]
    [InlineData("12345678909\n")]
    [InlineData("١٢٣٤٥")] // dígitos arábico-índicos (Unicode Nd)
    public void Validate_ShouldBeInvalid_WhenDocumentoIsNotOnlyDigits(string documento)
    {
        var result = _validator.Validate(new ObterClientePorDocumentoQuery(documento));

        Assert.False(result.IsValid);
    }
}
