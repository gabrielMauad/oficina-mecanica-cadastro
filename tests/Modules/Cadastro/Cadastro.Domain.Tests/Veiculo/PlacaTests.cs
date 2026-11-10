using Cadastro.Domain.Veiculo;
using SharedKernel.Domain;

namespace Cadastro.Domain.Tests.Veiculo;

public class PlacaTests
{
    [Theory]
    [InlineData("ABC1234")]
    [InlineData("ABC-1234")]
    [InlineData("ABC1D23")]
    public void Criar_ComValoresValidos_RetornaPlaca(string numero)
    {
        // Act
        var placaResult = Placa.Criar(numero);

        // Assert
        Assert.True(placaResult.IsSuccess);
        Assert.False(placaResult.IsFailure);
        Assert.Equal(Error.None, placaResult.Error);
        Assert.Equal(numero.Replace("-", ""), placaResult.Value.Numero);
    }

    [Theory]
    [InlineData("", "Placa.Invalida", "Placa é obrigatória.")]
    [InlineData("   ", "Placa.Invalida", "Placa é obrigatória.")]
    [InlineData("1234ABC", "Placa.Invalida", "Placa inválida.")]
    [InlineData("A1BCD23", "Placa.Invalida", "Placa inválida.")]
    [InlineData("ABC-12345", "Placa.Invalida", "Placa inválida.")]
    [InlineData("ABC_1234", "Placa.Invalida", "Placa inválida.")]
    public void Criar_ComValoresInvalidos_RetornaErro(string numero, string errorCode, string errorMessage)
    {
        // Act
        var placaResult = Placa.Criar(numero);

        // Assert
        Assert.False(placaResult.IsSuccess);
        Assert.True(placaResult.IsFailure);
        Assert.Equal(errorCode, placaResult.Error.Code);
        Assert.Equal(errorMessage, placaResult.Error.Message);
    }
}

