using Cadastro.Domain.Cliente;
using SharedKernel.Domain;
using VeiculoEntity = Cadastro.Domain.Veiculo.Veiculo;

namespace Cadastro.Domain.Tests.Veiculo;

public class VeiculoTests
{
    [Theory]
    [InlineData("ABC1234", "Modelo 1", "Marca 1", 2020)]
    [InlineData("ABC-1234", "Modelo 1", "Marca 1", 1900)]
    [InlineData("ABC1D23", "Modelo 1", "Marca 1", 1886)]
    public void Criar_ComDadosValidos_RetornaServico(string numPlaca, string modelo, string marca, int ano)
    {
        // Arrange
        var clienteId = new ClienteId(Guid.NewGuid());

        // Act
        var veiculoResult = VeiculoEntity.Criar(numPlaca, modelo, marca, ano, clienteId);

        // Assert
        Assert.True(veiculoResult.IsSuccess);
        Assert.False(veiculoResult.IsFailure);
        Assert.Equal(Error.None, veiculoResult.Error);
        Assert.Equal(numPlaca.Replace("-", ""), veiculoResult.Value.Placa.Numero);
        Assert.Equal(modelo, veiculoResult.Value.Modelo);
        Assert.Equal(marca, veiculoResult.Value.Marca);
        Assert.Equal(ano, veiculoResult.Value.Ano);
        Assert.Equal(clienteId, veiculoResult.Value.ClienteId);
        Assert.InRange(veiculoResult.Value.CadastradoEm, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
        Assert.InRange(veiculoResult.Value.AtualizadoEm, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
        Assert.NotEqual(Guid.Empty, veiculoResult.Value.Id.Value);
    }


    [Theory]
    [InlineData("ABC-1234", "", "Marca 1", 1900, true, "Veiculo.ModeloVazio", "Modelo é obrigatório.")]
    [InlineData("ABC-1234", "   ", "Marca 1", 1900, true, "Veiculo.ModeloVazio", "Modelo é obrigatório.")]
    [InlineData("ABC1D23", "Modelo 1", "", 1900, true, "Veiculo.MarcaVazia", "Marca é obrigatória.")]
    [InlineData("ABC1D23", "Modelo 1", "    ", 1900, true, "Veiculo.MarcaVazia", "Marca é obrigatória.")]
    [InlineData("ABC1D23", "Modelo 1", "Marca 1", 1885, true, "Veiculo.AnoInvalido", "Ano inválido.")]
    [InlineData("ABC1D23", "Modelo 1", "Marca 1", 0, true, "Veiculo.AnoInvalido", "Ano inválido.")]
    [InlineData("ABC1D23", "Modelo 1", "Marca 1", 1900, false, "Veiculo.ClienteIdVazio", "Id do cliente é obrigatório.")]
    [InlineData("", "Modelo 1", "Marca 1", 1900, true, "Placa.Invalida", "Placa é obrigatória.")]
    [InlineData("   ", "Modelo 1", "Marca 1", 1900, true, "Placa.Invalida", "Placa é obrigatória.")]
    [InlineData("1234", "Modelo 1", "Marca 1", 1900, true, "Placa.Invalida", "Placa inválida.")]
    public void Criar_ComDadosInvalidos_RetornaErro(
        string numPlaca,
        string modelo,
        string marca,
        int ano,
        bool hasClienteId,
        string errorCode,
        string errorMessage)
    {
        // Arrange
        if (ano == 0)
            ano = DateTime.Now.Year + 2;

        ClienteId? clienteId = hasClienteId ? new ClienteId(Guid.NewGuid()) : null;

        // Act
        var veiculoResult = VeiculoEntity.Criar(numPlaca, modelo, marca, ano, clienteId);

        // Assert
        Assert.False(veiculoResult.IsSuccess);
        Assert.True(veiculoResult.IsFailure);
        Assert.Equal(errorCode, veiculoResult.Error.Code);
        Assert.Equal(errorMessage, veiculoResult.Error.Message);
    }
}

