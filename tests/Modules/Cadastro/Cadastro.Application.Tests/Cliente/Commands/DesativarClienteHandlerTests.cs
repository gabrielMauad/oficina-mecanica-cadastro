using Cadastro.Application.Clientes.Commands.DesativarCliente;
using Cadastro.Application.Gateways;
using Cadastro.Domain.Cliente;
using Moq;
using SharedKernel.Domain;
using ClienteEntity = Cadastro.Domain.Cliente.Cliente;

namespace Cadastro.Application.Tests.Cliente.Commands;

public class DesativarClienteHandlerTests
{
    private readonly Mock<IClienteGateway> _gatewayMock;
    private readonly DesativarClienteHandler _handler;

    public DesativarClienteHandlerTests()
    {
        _gatewayMock = new Mock<IClienteGateway>();
        _handler = new DesativarClienteHandler(
            _gatewayMock.Object
        );
    }

    [Fact(DisplayName = "Cenário feliz")]
    public async Task Handle_ShouldReturnSucess_WhenCommandIsValid()
    {
        // Arrange
        var command = new DesativarClienteCommand(
            Guid.NewGuid()
        );
        var clienteId = new ClienteId(command.ClienteId);
        ClienteEntity? cliente = ClienteEntity.Criar("nome", "01404238000", "email@exemplo.com", "11999999999", true).Value;

        _gatewayMock.Setup(x => x.ObterPorId(clienteId, It.IsAny<CancellationToken>())).ReturnsAsync(cliente);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(Error.None, result.Error);

        Assert.NotEqual(Guid.Empty, result.Value.Id.Value);
        Assert.Equal(cliente.Nome, result.Value.Nome);
        Assert.False(result.Value.Ativo);
        Assert.InRange(result.Value.AtualizadoEm, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);

        _gatewayMock.Verify(x => x.Atualizar(It.Is<ClienteEntity>(x => x.Ativo == false), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Erro: Cliente nao encontrado")]
    public async Task Handle_ShouldReturnError_WhenClienteNotFound()
    {
        // Arrange
        var command = new DesativarClienteCommand(
            Guid.NewGuid()
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Cliente.NaoEncontrado", result.Error.Code);
        Assert.Equal("Cliente não encontrado.", result.Error.Message);
    }

    [Fact(DisplayName = "Idempotencia: Cliente desativado")]
    public async Task Handle_ShouldReturnError_WhenClienteDesativado()
    {
        // Arrange
        var command = new DesativarClienteCommand(
            Guid.NewGuid()
        );
        var clienteId = new ClienteId(command.ClienteId);
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
}
