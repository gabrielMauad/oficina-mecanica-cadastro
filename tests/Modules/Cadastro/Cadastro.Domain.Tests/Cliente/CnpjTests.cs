using Cadastro.Domain.Cliente;
using SharedKernel.Domain;

namespace Cadastro.Domain.Tests.Cliente;

public class CnpjTests
{
    [Theory]
    [InlineData("12.205.621/0001-90")]
    [InlineData("40843048000186")]
    public void Criar_ComDigitosValidos_RetornaCnpj(string cnpj)
    {
        // Act
        var cnpjResult = Cnpj.Criar(cnpj);

        // Assert
        Assert.True(cnpjResult.IsSuccess);
        Assert.False(cnpjResult.IsFailure);
        Assert.Equal(Error.None, cnpjResult.Error);
        Assert.Equal(new string(cnpj.Where(char.IsDigit).ToArray()), cnpjResult.Value.Numero);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Criar_ComStringVaziaOuEspacos_RetornaErro(string cnpj)
    {
        // Act
        var cnpjResult = Cnpj.Criar(cnpj);

        // Assert
        Assert.False(cnpjResult.IsSuccess);
        Assert.True(cnpjResult.IsFailure);
        Assert.Equal("CNPJ.Invalido", cnpjResult.Error.Code);
        Assert.Equal("CNPJ é obrigatório.", cnpjResult.Error.Message);
    }

    [Theory]
    [InlineData("12.345.678/9101-11")]
    [InlineData("12345678910111")]
    public void Criar_ComDigitosInvalidos_RetornaErro(string cnpj)
    {
        // Act
        var cnpjResult = Cnpj.Criar(cnpj);

        // Assert
        Assert.False(cnpjResult.IsSuccess);
        Assert.True(cnpjResult.IsFailure);
        Assert.Equal("CNPJ.Invalido", cnpjResult.Error.Code);
        Assert.Equal("CNPJ inválido.", cnpjResult.Error.Message);
    }
}
