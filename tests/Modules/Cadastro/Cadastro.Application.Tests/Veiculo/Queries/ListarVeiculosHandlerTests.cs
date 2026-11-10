using Cadastro.Application.Veiculos.Queries.ListarVeiculos;
using Moq;

namespace Cadastro.Application.Tests.Veiculo.Queries;

public class ListarVeiculosHandlerTests
{
    private readonly Mock<IListarVeiculosQuery> _queryMock;
    private readonly ListarVeiculosHandler _handler;

    public ListarVeiculosHandlerTests()
    {
        _queryMock = new Mock<IListarVeiculosQuery>();
        _handler = new ListarVeiculosHandler(
            _queryMock.Object
        );
    }

    [Fact(DisplayName = "Cenário feliz")]
    public async Task Handle_ShouldReturnSucess_WhenQueryIsValid()
    {
        // Arrange
        var query = new ListarVeiculosQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _queryMock.Verify(x => x.Listar(It.IsAny<CancellationToken>()), Times.Once);
    }
}

