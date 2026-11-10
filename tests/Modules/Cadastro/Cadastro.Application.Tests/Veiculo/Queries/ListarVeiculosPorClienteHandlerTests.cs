using Cadastro.Application.Veiculos.Queries.ListarVeiculosPorCliente;
using Moq;
using SharedKernel.Domain;

namespace Cadastro.Application.Tests.Veiculo.Queries;

public class ListarVeiculosPorClienteHandlerTests
{
    private readonly Mock<IListarVeiculosPorClienteQuery> _queryMock;
    private readonly ListarVeiculosPorClienteHandler _handler;

    public ListarVeiculosPorClienteHandlerTests()
    {
        _queryMock = new Mock<IListarVeiculosPorClienteQuery>();
        _handler = new ListarVeiculosPorClienteHandler(
            _queryMock.Object
        );
    }

    [Fact(DisplayName = "Cenário feliz")]
    public async Task Handle_ShouldReturnSucess_WhenQueryIsValid()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var query = new ListarVeiculosPorClienteQuery(clienteId);
        var veiculo = new VeiculoDoCliente(
            Guid.NewGuid(),
            "ABC-1234",
            "Fiat Uno",
            "Fiat",
            2026,
            DateTime.Today,
            DateTime.Today
        );
        var veiculos = new List<VeiculoDoCliente> { veiculo };
        var veiculosPorCliente = new VeiculosPorCliente(
            clienteId,
            "João Silva",
            veiculos
        );

        _queryMock.Setup(x => x.ListarPorClienteId(clienteId, It.IsAny<CancellationToken>())).ReturnsAsync(veiculosPorCliente);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(Error.None, result.Error);

        Assert.Equal(clienteId, result.Value.ClienteId);
        Assert.Equal("João Silva", result.Value.NomeCliente);
        Assert.Single(result.Value.Veiculos);
    }

    [Fact(DisplayName = "Cliente não encontrado")]
    public async Task Handle_ShouldReturnFailure_WhenClienteNotFound()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var query = new ListarVeiculosPorClienteQuery(clienteId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Veiculo.ClienteNaoEncontrado", result.Error.Code);
        Assert.Equal("Cliente não encontrado.", result.Error.Message);
    }
}

