using Cadastro.Application.Servicos.Queries.ListarServicos;
using Moq;

namespace Cadastro.Application.Tests.Servico.Queries;

public class ListarServicosHandlerTests
{
    private readonly Mock<IListarServicosQuery> _queryMock;
    private readonly ListarServicosHandler _handler;

    public ListarServicosHandlerTests()
    {
        _queryMock = new Mock<IListarServicosQuery>();
        _handler = new ListarServicosHandler(_queryMock.Object);
    }

    [Fact(DisplayName = "Cenário Feliz")]
    public async Task Handle_ShouldReturnSuccess_WhenQueryIsValid()
    {
        // Arrange
        var query = new ListarServicosQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _queryMock.Verify(x => x.Listar(It.IsAny<CancellationToken>()), Times.Once);
    }
}
