using Cadastro.Application.Clientes.Commands.AtualizarCliente;
using Cadastro.Application.Gateways;
using Cadastro.Domain.Cliente;
using Moq;
using SharedKernel.Domain;
using ClienteEntity = Cadastro.Domain.Cliente.Cliente;

namespace Cadastro.Application.Tests.Cliente.Commands;

public class AtualizarClienteHandlerTests
{
    private readonly Mock<IClienteGateway> _gatewayMock;
    private readonly AtualizarClienteHandler _handler;

    public AtualizarClienteHandlerTests()
    {
        _gatewayMock = new Mock<IClienteGateway>();
        _handler = new AtualizarClienteHandler(
            _gatewayMock.Object
        );
    }

    [Theory(DisplayName = "Cenário feliz")]
    [InlineData("Nome atualizado", null)]
    [InlineData(null, "31999999999")]
    [InlineData("Nome atualizado", "31999999999")]
    public async Task Handle_ShouldReturnSucess_WhenCommandIsValid(string? nome, string? telefone)
    {
        // Arrange
        var command = new AtualizarClienteCommand(
            Guid.NewGuid(),
            nome,
            telefone
        );
        var clienteId = new ClienteId(command.Id);
        ClienteEntity? cliente = ClienteEntity.Criar("nome", "01404238000", "email@exemplo.com", "11999999999", true).Value;
        var nomeEsperado = nome ?? cliente.Nome;
        var telefoneEsperado = telefone ?? cliente.Telefone;

        _gatewayMock.Setup(x => x.ObterPorId(clienteId, It.IsAny<CancellationToken>())).ReturnsAsync(cliente);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(Error.None, result.Error);

        Assert.NotEqual(Guid.Empty, result.Value.Id.Value);
        Assert.Equal(nomeEsperado, result.Value.Nome);
        Assert.Equal(telefoneEsperado, result.Value.Telefone);
        Assert.True(result.Value.Ativo);
        Assert.InRange(result.Value.AtualizadoEm, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);

        _gatewayMock.Verify(x => x.Atualizar(It.Is<ClienteEntity>(x => x.Nome == nomeEsperado), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Erro: Cliente nao encontrado")]
    public async Task Handle_ShouldReturnError_WhenClienteNotFound()
    {
        // Arrange
        var command = new AtualizarClienteCommand(
            Guid.NewGuid(),
            "nome",
            "31999999999"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Cliente.NaoEncontrado", result.Error.Code);
        Assert.Equal("Cliente não encontrado.", result.Error.Message);
    }

    [Fact(DisplayName = "Erro: Cliente desativado")]
    public async Task Handle_ShouldReturnError_WhenClienteDesativado()
    {
        // Arrange
        var command = new AtualizarClienteCommand(
            Guid.NewGuid(),
            "nome",
            "31999999999"
        );
        var clienteId = new ClienteId(command.Id);
        ClienteEntity? cliente = ClienteEntity.Criar("nome", "01404238000", "email@exemplo.com", "11999999999", true).Value;
        cliente.Desativar();

        _gatewayMock.Setup(x => x.ObterPorId(clienteId, It.IsAny<CancellationToken>())).ReturnsAsync(cliente);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Cliente.JaDesativado", result.Error.Code);
        Assert.Equal("O cliente já está desativado.", result.Error.Message);
    }

    [Fact(DisplayName = "Idempotencia: cliente sem alteracoes")]
    public async Task Handle_ShouldReturnError_WhenClienteFails()
    {
        // Arrange
        var command = new AtualizarClienteCommand(
            Guid.NewGuid(),
            "nome",
            "11999999999"
        );
        var clienteId = new ClienteId(command.Id);
        ClienteEntity? cliente = ClienteEntity.Criar("nome", "01404238000", "email@exemplo.com", "11999999999", true).Value;

        _gatewayMock.Setup(x => x.ObterPorId(clienteId, It.IsAny<CancellationToken>())).ReturnsAsync(cliente);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(Error.None, result.Error);

        _gatewayMock.Verify(x => x.Atualizar(It.Is<ClienteEntity>(x => x.Nome == "nome"), It.IsAny<CancellationToken>()), Times.Never);
    }
}
