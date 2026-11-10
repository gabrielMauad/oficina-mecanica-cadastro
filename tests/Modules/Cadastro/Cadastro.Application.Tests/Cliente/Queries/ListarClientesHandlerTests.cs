using Cadastro.Application.Clientes.Queries.ListarClientes;
using Moq;

namespace Cadastro.Application.Tests.Cliente.Queries;

public class ListarClientesHandlerTests
{
    private readonly Mock<IListarClientesQuery> _queryMock;
    private readonly ListarClientesHandler _handler;

    public ListarClientesHandlerTests()
    {
        _queryMock = new Mock<IListarClientesQuery>();
        _handler = new ListarClientesHandler(
            _queryMock.Object
        );
    }

    [Fact(DisplayName = "Cenário feliz")]
    public async Task Handle_ShouldReturnSucess_WhenQueryIsValid()
    {
        // Arrange
        var query = new ListarClientesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _queryMock.Verify(x => x.Listar(It.IsAny<CancellationToken>()), Times.Once);
    }
}