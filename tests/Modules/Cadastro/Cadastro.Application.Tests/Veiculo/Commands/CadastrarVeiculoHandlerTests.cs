using Cadastro.Application.Gateways;
using Cadastro.Application.Veiculos.Commands.CadastrarVeiculo;
using Cadastro.Domain.Cliente;
using Cadastro.Domain.Veiculo;
using Moq;
using SharedKernel.Domain;
using ClienteEntity = Cadastro.Domain.Cliente.Cliente;
using VeiculoEntity = Cadastro.Domain.Veiculo.Veiculo;

namespace Cadastro.Application.Tests.Veiculo.Commands;

public class CadastrarVeiculoHandlerTests
{
    private readonly Mock<IVeiculoGateway> _gatewayMock;
    private readonly Mock<IClienteGateway> _clienteGatewayMock;
    private readonly CadastrarVeiculoHandler _handler;

    public CadastrarVeiculoHandlerTests()
    {
        _gatewayMock = new Mock<IVeiculoGateway>();
        _clienteGatewayMock = new Mock<IClienteGateway>();
        _handler = new CadastrarVeiculoHandler(_gatewayMock.Object, _clienteGatewayMock.Object);
    }

    [Fact(DisplayName = "Cenário feliz")]
    public async Task Handle_ShouldReturnSucess_WhenCommandIsValid()
    {
        // Arrange
        var clienteIdGuid = Guid.NewGuid();
        var command = new CadastrarVeiculoCommand(
            Placa: "ABC1234",
            Modelo: "Modelo",
            Marca: "Marca",
            Ano: 2026,
            ClienteId: clienteIdGuid
        );
        var clienteId = new ClienteId(command.ClienteId);
        ClienteEntity? cliente = ClienteEntity.Criar("nome", "01404238000", "email@exemplo.com", "11999999999", true).Value;
        var placaNormalizada = command.Placa.ToUpperInvariant().Replace("-", "");

        _gatewayMock.Setup(x => x.ExistePorPlaca(placaNormalizada, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _clienteGatewayMock.Setup(x => x.ObterPorId(clienteId, It.IsAny<CancellationToken>())).ReturnsAsync(cliente);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(Error.None, result.Error);

        Assert.NotEqual(Guid.Empty, result.Value.Id.Value);
        Assert.Equal(placaNormalizada, result.Value.Placa.Numero);
        Assert.Equal(command.Modelo, result.Value.Modelo);
        Assert.Equal(command.Marca, result.Value.Marca);
        Assert.Equal(command.Ano, result.Value.Ano);
        Assert.Equal(command.ClienteId, result.Value.ClienteId.Value);
        Assert.InRange(result.Value.CadastradoEm, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
        Assert.InRange(result.Value.AtualizadoEm, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);

        _gatewayMock.Verify(x => x.Adicionar(It.Is<VeiculoEntity>(x => x.Placa.Numero == placaNormalizada), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Erro: Documento ja existe")]
    public async Task Handle_ShouldReturnError_WhenDocumentoAlreadyExists()
    {
        var clienteIdGuid = Guid.NewGuid();
        var command = new CadastrarVeiculoCommand(
            Placa: "ABC1234",
            Modelo: "Modelo",
            Marca: "Marca",
            Ano: 2026,
            ClienteId: clienteIdGuid
        );
        var placaNormalizada = command.Placa.ToUpperInvariant().Replace("-", "");

        _gatewayMock.Setup(x => x.ExistePorPlaca(placaNormalizada, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Veiculo.PlacaJaExiste", result.Error.Code);
        Assert.Equal("Já existe um veículo cadastrado com esta placa.", result.Error.Message);
    }

    [Fact(DisplayName = "Erro: Cliente não encontrado")]
    public async Task Handle_ShouldReturnError_WhenClienteNotFound()
    {
        var clienteIdGuid = Guid.NewGuid();
        var command = new CadastrarVeiculoCommand(
            Placa: "ABC1234",
            Modelo: "Modelo",
            Marca: "Marca",
            Ano: 2026,
            ClienteId: clienteIdGuid
        );
        var placaNormalizada = command.Placa.ToUpperInvariant().Replace("-", "");

        _gatewayMock.Setup(x => x.ExistePorPlaca(placaNormalizada, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Veiculo.ClienteNaoEncontrado", result.Error.Code);
        Assert.Equal("Cliente não encontrado.", result.Error.Message);
    }

    [Fact(DisplayName = "Erro: Cliente inativo")]
    public async Task Handle_ShouldReturnError_WhenClienteIsInactive()
    {
        // Arrange
        var clienteIdGuid = Guid.NewGuid();
        var command = new CadastrarVeiculoCommand(
            Placa: "ABC1234",
            Modelo: "Modelo",
            Marca: "Marca",
            Ano: 2026,
            ClienteId: clienteIdGuid
        );
        var clienteId = new ClienteId(command.ClienteId);
        ClienteEntity cliente = ClienteEntity.Criar("nome", "01404238000", "email@exemplo.com", "11999999999", true).Value;
        cliente.Desativar();

        var placaNormalizada = command.Placa.ToUpperInvariant().Replace("-", "");

        _gatewayMock.Setup(x => x.ExistePorPlaca(placaNormalizada, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _clienteGatewayMock.Setup(x => x.ObterPorId(clienteId, It.IsAny<CancellationToken>())).ReturnsAsync(cliente);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Veiculo.ClienteInativo", result.Error.Code);
        Assert.Equal("Não é possível cadastrar veículo para um cliente inativo.", result.Error.Message);
    }

    [Fact(DisplayName = "Replica erro do dominio")]
    public async Task Handle_ShouldReturnError_WhenVeiculoFails()
    {
        // Arrange
        var clienteIdGuid = Guid.NewGuid();
        var command = new CadastrarVeiculoCommand(
            Placa: "Placa Inválida",
            Modelo: "Modelo",
            Marca: "Marca",
            Ano: 2026,
            ClienteId: clienteIdGuid
        );
        var clienteId = new ClienteId(command.ClienteId);
        ClienteEntity? cliente = ClienteEntity.Criar("nome", "01404238000", "email@exemplo.com", "11999999999", true).Value;

        var placaNormalizada = command.Placa.ToUpperInvariant().Replace("-", "");

        _gatewayMock.Setup(x => x.ExistePorPlaca(placaNormalizada, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _clienteGatewayMock.Setup(x => x.ObterPorId(clienteId, It.IsAny<CancellationToken>())).ReturnsAsync(cliente);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Placa.Invalida", result.Error.Code);
        Assert.Equal("Placa inválida.", result.Error.Message);
    }
}
