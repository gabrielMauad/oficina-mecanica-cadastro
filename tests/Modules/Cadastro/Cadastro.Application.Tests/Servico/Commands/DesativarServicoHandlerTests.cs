using Cadastro.Application.Gateways;
using Cadastro.Application.Servicos.Commands.DesativarServico;
using Cadastro.Domain.Servico;
using Moq;
using SharedKernel.Domain;
using ServicoEntity = Cadastro.Domain.Servico.Servico;

namespace Cadastro.Application.Tests.Servico.Commands;

public class DesativarServicoHandlerTests
{
    private readonly Mock<IServicoGateway> _gatewayMock;
    private readonly DesativarServicoHandler _handler;

    public DesativarServicoHandlerTests()
    {
        _gatewayMock = new Mock<IServicoGateway>();
        _handler = new DesativarServicoHandler(
            _gatewayMock.Object
        );
    }

    [Fact(DisplayName = "Cenário Feliz")]
    public async Task Handle_ShouldReturnSuccess_WhenCommandIsValid()
    {
        // Arrange
        var command = new DesativarServicoCommand(
            Guid.NewGuid()
        );
        var servicoId = new ServicoId(command.ServicoId);
        ServicoEntity? servico = ServicoEntity.Criar("Servico", "Descricao inicial", 0).Value;

        _gatewayMock.Setup(x => x.ObterPorId(servicoId, It.IsAny<CancellationToken>())).ReturnsAsync(servico);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(Error.None, result.Error);

        Assert.NotEqual(Guid.Empty, result.Value.Id.Value);
        Assert.Equal("Servico", result.Value.Nome);
        Assert.False(result.Value.Ativo);
        _gatewayMock.Verify(x => x.Atualizar(It.Is<ServicoEntity>(x => x.Nome == "Servico" && !x.Ativo), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Erro: Servico não encontrado")]
    public async Task Handle_ShouldReturnError_WhenServicoNotFound()
    {
        // Arrange
        var command = new DesativarServicoCommand(
            Guid.NewGuid()
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Servico.NaoEncontrado", result.Error.Code);
        Assert.Equal("Servico não encontrado.", result.Error.Message);
        _gatewayMock.Verify(x => x.Atualizar(It.IsAny<ServicoEntity>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact(DisplayName = "Idempotencia")]
    public async Task Handle_ShouldDoNothing_WhenServicoAlreadyInactive()
    {
        // Arrange
        var command = new DesativarServicoCommand(
            Guid.NewGuid()
        );
        var servicoId = new ServicoId(command.ServicoId);
        ServicoEntity? servico = ServicoEntity.Criar("Servico", "Descricao inicial", 0).Value;
        servico.Desativar();

        _gatewayMock.Setup(x => x.ObterPorId(servicoId, It.IsAny<CancellationToken>())).ReturnsAsync(servico);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Servico.JaDesativado", result.Error.Code);
        Assert.Equal("O servico já está desativado.", result.Error.Message);
        _gatewayMock.Verify(x => x.Atualizar(It.IsAny<ServicoEntity>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
