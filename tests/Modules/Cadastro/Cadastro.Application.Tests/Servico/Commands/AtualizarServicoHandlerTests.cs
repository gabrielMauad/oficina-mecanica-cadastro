using Cadastro.Application.Gateways;
using Cadastro.Application.Servicos.Commands.AtualizarServico;
using Cadastro.Domain.Servico;
using Moq;
using SharedKernel.Domain;
using ServicoEntity = Cadastro.Domain.Servico.Servico;

namespace Cadastro.Application.Tests.Servico.Commands;

public class AtualizarServicoHandlerTests
{
    private readonly Mock<IServicoGateway> _gatewayMock;
    private readonly AtualizarServicoHandler _handler;

    public AtualizarServicoHandlerTests()
    {
        _gatewayMock = new Mock<IServicoGateway>();
        _handler = new AtualizarServicoHandler(
            _gatewayMock.Object
        );
    }

    [Theory(DisplayName = "Cenário feliz")]
    [InlineData("Descricao atualizada", null)]
    [InlineData(null, 1d)]
    [InlineData("Descricao atualizada", 1d)]
    public async Task Handle_ShouldReturnSucess_WhenCommandIsValid(string? descricao, double? preco)
    {
        // Arrange
        decimal? precoDecimal = preco.HasValue ? (decimal)preco.Value : null;
        var command = new AtualizarServicoCommand(
            Guid.NewGuid(),
            descricao,
            precoDecimal
        );
        var servicoId = new ServicoId(command.ServicoId);
        ServicoEntity? servico = ServicoEntity.Criar("Servico", "Descricao inicial", 0).Value;
        var descricaoEsperada = descricao ?? servico.Descricao;
        var precoEsperado = precoDecimal ?? servico.PrecoBase.Valor;

        _gatewayMock.Setup(x => x.ObterPorId(servicoId, It.IsAny<CancellationToken>())).ReturnsAsync(servico);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(Error.None, result.Error);

        Assert.NotEqual(Guid.Empty, result.Value.Id.Value);
        Assert.Equal(descricaoEsperada, result.Value.Descricao);
        Assert.Equal(precoEsperado, result.Value.PrecoBase.Valor);
        Assert.True(result.Value.Ativo);
        Assert.InRange(result.Value.AtualizadoEm, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);

        _gatewayMock.Verify(x => x.Atualizar(It.Is<ServicoEntity>(x => x.Descricao == descricaoEsperada && x.PrecoBase.Valor == precoEsperado), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Erro: Servico nao encontrado")]
    public async Task Handle_ShouldReturnError_WhenServicoNotFound()
    {
        // Arrange
        var command = new AtualizarServicoCommand(
            Guid.NewGuid(),
            "Descricao",
            100
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Servico.NaoEncontrado", result.Error.Code);
        Assert.Equal("Servico não encontrado.", result.Error.Message);
    }

    [Fact(DisplayName = "Erro: Servico desativado")]
    public async Task Handle_ShouldReturnError_WhenServicoDesativado()
    {
        // Arrange
        var command = new AtualizarServicoCommand(
            Guid.NewGuid(),
            "Descricao",
            100
        );
        var servicoId = new ServicoId(command.ServicoId);
        ServicoEntity? servico = ServicoEntity.Criar("Servico", command.Descricao, 0).Value;
        servico.Desativar();
        _gatewayMock.Setup(x => x.ObterPorId(servicoId, It.IsAny<CancellationToken>())).ReturnsAsync(servico);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Servico.JaDesativado", result.Error.Code);
        Assert.Equal("O servico já está desativado.", result.Error.Message);
    }

    [Fact(DisplayName = "Erro: Replica erro de domínio")]
    public async Task Handle_ShouldReturnError_WhenDomainFails()
    {
        // Arrange
        var command = new AtualizarServicoCommand(
            Guid.NewGuid(),
            "Descricao",
            -1
        );
        var servicoId = new ServicoId(command.ServicoId);
        ServicoEntity? servico = ServicoEntity.Criar("Servico", command.Descricao, 0).Value;
        _gatewayMock.Setup(x => x.ObterPorId(servicoId, It.IsAny<CancellationToken>())).ReturnsAsync(servico);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Dinheiro.Negativo", result.Error.Code);
        Assert.Equal("Preço não pode ser negativo.", result.Error.Message);
    }

    [Fact(DisplayName = "Idempotencia: servico sem alteracoes")]
    public async Task Handle_ShouldDoNothing_WhenServicoHasNoChanges()
    {
        // Arrange
        var command = new AtualizarServicoCommand(
            Guid.NewGuid(),
            "Descricao",
            100
        );
        var servicoId = new ServicoId(command.ServicoId);
        ServicoEntity? servico = ServicoEntity.Criar("Servico", command.Descricao, 100).Value;
        _gatewayMock.Setup(x => x.ObterPorId(servicoId, It.IsAny<CancellationToken>())).ReturnsAsync(servico);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(Error.None, result.Error);

        _gatewayMock.Verify(x => x.Atualizar(It.Is<ServicoEntity>(x => x.Nome == "Servico"), It.IsAny<CancellationToken>()), Times.Never);
    }
}

