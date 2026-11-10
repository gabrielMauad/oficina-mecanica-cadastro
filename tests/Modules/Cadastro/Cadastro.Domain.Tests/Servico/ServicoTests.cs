using SharedKernel.Domain;
using ServicoEntity = Cadastro.Domain.Servico.Servico;

namespace Cadastro.Domain.Tests.Servico;

public class ServicoTests
{
    [Theory]
    [InlineData("Servico 1", "Descricao 1", 100.0)]
    [InlineData("Servico 1", null, 0)]
    [InlineData("Servico 1", "", 0.1)]
    [InlineData("Servico 1", "  ", 100.1)]
    public void Criar_ComDadosValidos_RetornaServico(string nome, string? descricao, decimal preco)
    {
        // Act
        var servicoResult = ServicoEntity.Criar(nome, descricao, preco);

        // Assert
        Assert.True(servicoResult.IsSuccess);
        Assert.False(servicoResult.IsFailure);
        Assert.Equal(Error.None, servicoResult.Error);
        Assert.Equal(nome, servicoResult.Value.Nome);
        Assert.Equal(descricao, servicoResult.Value.Descricao);
        Assert.Equal(preco, servicoResult.Value.PrecoBase.Valor);
        Assert.True(servicoResult.Value.Ativo);
        Assert.InRange(servicoResult.Value.CadastradoEm, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
        Assert.InRange(servicoResult.Value.AtualizadoEm, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
        Assert.NotEqual(Guid.Empty, servicoResult.Value.Id.Value);
    }


    [Theory]
    [InlineData("", "Descricao 1", 100.0, "Servico.NomeVazio", "Nome é obrigatório.")]
    [InlineData("   ", "Descricao 1", 100.0, "Servico.NomeVazio", "Nome é obrigatório.")]
    [InlineData("Servico 1", null, -1, "Dinheiro.Negativo", "Preço não pode ser negativo.")]
    [InlineData("Servico 1", "", -0.00001, "Dinheiro.Negativo", "Preço não pode ser negativo.")]
    public void Criar_ComDadosInvalidos_RetornaErro(string nome, string? descricao, decimal preco, string errorCode, string errorMessage)
    {
        // Act
        var servicoResult = ServicoEntity.Criar(nome, descricao, preco);

        // Assert
        Assert.False(servicoResult.IsSuccess);
        Assert.True(servicoResult.IsFailure);
        Assert.Equal(errorCode, servicoResult.Error.Code);
        Assert.Equal(errorMessage, servicoResult.Error.Message);
    }

    [Fact]
    public void AtualizarDescricao_ComNovaDescricao_AtualizaDescricao()
    {
        // Arrange
        var servico = ServicoEntity.Criar("Servico 1", "desc antiga", 100.0m).Value;

        // Act
        var servicoResult = servico.AtualizarDescricao("desc nova");

        // Assert
        Assert.True(servicoResult.IsSuccess);
        Assert.Equal("desc nova", servico.Descricao);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1.5)]
    [InlineData(999.99)]
    public void AtualizarPrecoBase_ComPrecoValido_AtualizaPreco(decimal novoPreco)
    {
        // Arrange
        var servico = ServicoEntity.Criar("Servico 1", null, 100.0m).Value;

        // Act
        var servicoResult = servico.AtualizarPrecoBase(novoPreco);

        // Assert
        Assert.True(servicoResult.IsSuccess);
        Assert.Equal(novoPreco, servico.PrecoBase.Valor);
    }

    [Theory]
    [InlineData(-1, "Dinheiro.Negativo", "Preço não pode ser negativo.")]
    [InlineData(-0.0001, "Dinheiro.Negativo", "Preço não pode ser negativo.")]
    public void AtualizarPrecoBase_ComPrecoNegativo_RetornaErro(decimal novoPreco, string errorCode, string errorMessage)
    {
        // Arrange
        var servico = ServicoEntity.Criar("Servico 1", null, 100.0m).Value;

        // Act
        var servicoResult = servico.AtualizarPrecoBase(novoPreco);

        // Assert
        Assert.False(servicoResult.IsSuccess);
        Assert.True(servicoResult.IsFailure);
        Assert.Equal(errorCode, servicoResult.Error.Code);
        Assert.Equal(errorMessage, servicoResult.Error.Message);
    }

    [Fact]
    public void Desativar_ServicoAtivo_SetaAtivoComoFalso()
    {
        // Arrange
        var servico = ServicoEntity.Criar("Servico 1", null, 100.0m).Value;

        // Act
        servico.Desativar();

        // Assert
        Assert.False(servico.Ativo);
    }
}

