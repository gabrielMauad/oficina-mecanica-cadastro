using Cadastro.Application.Gateways;
using Cadastro.Application.Servicos.Queries.ObterServicoPorId;
using Cadastro.Domain.Servico;
using Moq;
using SharedKernel.Domain;
using ServicoEntity = Cadastro.Domain.Servico.Servico;

namespace Cadastro.Application.Tests.Servico.Queries;

public class ObterServicoPorIdHandlerTests
{
    private readonly Mock<IServicoGateway> _gatewayMock;
    private readonly ObterServicoPorIdHandler _handler;

    public ObterServicoPorIdHandlerTests()
    {
        _gatewayMock = new Mock<IServicoGateway>();
        _handler = new ObterServicoPorIdHandler(_gatewayMock.Object);
    }

    [Fact(DisplayName = "Cenário Feliz")]
    public async Task Handle_ShouldReturnSuccess_WhenQueryIsValid()
    {
        // Arrange
        var query = new ObterServicoPorIdQuery(
            Guid.NewGuid()
        );
        var servicoId = new ServicoId(query.ServicoId);
        ServicoEntity? servico = ServicoEntity.Criar("Servico", "Descricao inicial", 0).Value;

        _gatewayMock.Setup(x => x.ObterPorId(servicoId, It.IsAny<CancellationToken>())).ReturnsAsync(servico);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(Error.None, result.Error);

        Assert.Equal(servico.Id.Value, result.Value.Id.Value);
        Assert.Equal("Servico", result.Value.Nome);
        Assert.Equal("Descricao inicial", result.Value.Descricao);
        Assert.Equal(0, result.Value.PrecoBase.Valor);
        Assert.True(result.Value.Ativo);
        Assert.Equal(servico.CadastradoEm, result.Value.CadastradoEm);
        Assert.Equal(servico.AtualizadoEm, result.Value.AtualizadoEm);
    }

    [Fact(DisplayName = "Error: Servico não encontrado")]
    public async Task Handle_ShouldReturnError_WhenServicoNotFound()
    {
        // Arrange
        var query = new ObterServicoPorIdQuery(
            Guid.NewGuid()
        );

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Servico.NaoEncontrado", result.Error.Code);
        Assert.Equal("Servico não encontrado.", result.Error.Message);
    }
}
