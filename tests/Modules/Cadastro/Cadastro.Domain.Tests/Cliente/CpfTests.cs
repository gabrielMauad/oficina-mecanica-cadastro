using Cadastro.Domain.Cliente;
using SharedKernel.Domain;

namespace Cadastro.Domain.Tests.Cliente;

public class CpfTests
{
    [Theory]
    [InlineData("632.582.650-70")]
    [InlineData("862.474.516-01")]
    [InlineData("01404238000")]
    [InlineData("27190094660")]
    public void Criar_ComDigitosValidos_RetornaCpf(string cpf)
    {
        // Act
        var cpfResult = Cpf.Criar(cpf);

        // Assert
        Assert.True(cpfResult.IsSuccess);
        Assert.False(cpfResult.IsFailure);
        Assert.Equal(Error.None, cpfResult.Error);
        Assert.Equal(new string(cpf.Where(char.IsDigit).ToArray()), cpfResult.Value.Numero);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Criar_ComStringVaziaOuEspacos_RetornaErro(string cpf)
    {
        // Act
        var cpfResult = Cpf.Criar(cpf);

        // Assert
        Assert.False(cpfResult.IsSuccess);
        Assert.True(cpfResult.IsFailure);
        Assert.Equal("CPF.Invalido", cpfResult.Error.Code);
        Assert.Equal("CPF é obrigatório.", cpfResult.Error.Message);
    }

    [Theory]
    [InlineData("123.456.789-10")]
    [InlineData("12345678910")]
    public void Criar_ComDigitosInvalidos_RetornaErro(string cpf)
    {
        // Act
        var cpfResult = Cpf.Criar(cpf);

        // Assert
        Assert.False(cpfResult.IsSuccess);
        Assert.True(cpfResult.IsFailure);
        Assert.Equal("CPF.Invalido", cpfResult.Error.Code);
        Assert.Equal("CPF inválido.", cpfResult.Error.Message);
    }
}

