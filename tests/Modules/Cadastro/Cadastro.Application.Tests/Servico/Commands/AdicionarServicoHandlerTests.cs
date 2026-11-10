using Cadastro.Application.Gateways;
using Cadastro.Application.Servicos.Commands.AdicionarServico;
using Cadastro.Domain.Servico;
using Moq;
using SharedKernel.Domain;
using ServicoEntity = Cadastro.Domain.Servico.Servico;

namespace Cadastro.Application.Tests.Servico.Commands;

public class AdicionarServicoHandlerTests
{
    private readonly Mock<IServicoGateway> _gatewayMock;
    private readonly AdicionarServicoHandler _handler;

    public AdicionarServicoHandlerTests()
    {
        _gatewayMock = new Mock<IServicoGateway>();
        _handler = new AdicionarServicoHandler(
            _gatewayMock.Object
        );
    }

    [Fact(DisplayName = "Cenário feliz")]
    public async Task Handle_ShouldReturnSucess_WhenCommandIsValid()
    {
        // Arrange
        var command = new AdicionarServicoCommand(
            Nome: "Serviço 1",
            Descricao: "Descrição do serviço",
            Preco: 100
        );

        _gatewayMock.Setup(x => x.ExistePorNome(command.Nome, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(Error.None, result.Error);

        Assert.NotEqual(Guid.Empty, result.Value.Id.Value);
        Assert.Equal(command.Nome, result.Value.Nome);
        Assert.Equal(command.Descricao, result.Value.Descricao);
        Assert.Equal(command.Preco, result.Value.PrecoBase.Valor);
        Assert.True(result.Value.Ativo);
        Assert.InRange(result.Value.CadastradoEm, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
        Assert.InRange(result.Value.AtualizadoEm, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);

        _gatewayMock.Verify(x => x.Adicionar(It.Is<ServicoEntity>(x => x.Nome == command.Nome), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact(DisplayName = "Erro: Servico ja existe")]
    public async Task Handle_ShouldReturnError_WhenServicoAlreadyExists()
    {
        // Arrange
        var command = new AdicionarServicoCommand(
            Nome: "Serviço 1",
            Descricao: "Descrição do serviço",
            Preco: 100
        );

        _gatewayMock.Setup(x => x.ExistePorNome(command.Nome, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Servico.NomeJaExiste", result.Error.Code);
        Assert.Equal("Já existe um servico cadastrado com este nome.", result.Error.Message);
    }

    [Fact(DisplayName = "Replica erro do dominio")]
    public async Task Handle_ShouldReturnError_WhenClienteFails()
    {
        // Arrange
        var command = new AdicionarServicoCommand(
            Nome: "",
            Descricao: "Descrição do serviço",
            Preco: 100
        );

        _gatewayMock.Setup(x => x.ExistePorNome(command.Nome, It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Servico.NomeVazio", result.Error.Code);
        Assert.Equal("Nome é obrigatório.", result.Error.Message);
    }
}

