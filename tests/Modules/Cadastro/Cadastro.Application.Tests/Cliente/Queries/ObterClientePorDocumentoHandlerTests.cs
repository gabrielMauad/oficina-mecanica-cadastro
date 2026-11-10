using Cadastro.Application.Clientes.Queries.ObterClientePorDocumento;
using Cadastro.Application.Gateways;
using Moq;
using SharedKernel.Domain;
using ClienteEntity = Cadastro.Domain.Cliente.Cliente;

namespace Cadastro.Application.Tests.Cliente.Queries;

public class ObterClientePorDocumentoHandlerTests
{
    private const string Documento = "01404238000";

    private readonly Mock<IClienteGateway> _gatewayMock;
    private readonly ObterClientePorDocumentoHandler _handler;

    public ObterClientePorDocumentoHandlerTests()
    {
        _gatewayMock = new Mock<IClienteGateway>();
        _handler = new ObterClientePorDocumentoHandler(_gatewayMock.Object);
    }

    [Fact(DisplayName = "Cenário feliz")]
    public async Task Handle_ShouldReturnSuccess_WhenClienteExists()
    {
        // Arrange
        var cliente = ClienteEntity.Criar("nome", Documento, "email@exemplo.com", "11999999999", true).Value;
        _gatewayMock
            .Setup(x => x.ObterPorDocumento(Documento, It.IsAny<CancellationToken>()))
            .ReturnsAsync(cliente);

        // Act
        var result = await _handler.Handle(new ObterClientePorDocumentoQuery(Documento), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(Error.None, result.Error);
        Assert.Equal(cliente.Id, result.Value.Id);
        Assert.Equal(cliente.Nome, result.Value.Nome);
        Assert.Equal(Documento, result.Value.Documento.Numero);
        Assert.Equal(cliente.Email, result.Value.Email);
        Assert.Equal(cliente.Telefone, result.Value.Telefone);
        Assert.True(result.Value.Ativo);
    }

    [Fact(DisplayName = "Erro: Cliente nao encontrado")]
    public async Task Handle_ShouldReturnError_WhenClienteNotFound()
    {
        // Arrange
        _gatewayMock
            .Setup(x => x.ObterPorDocumento(Documento, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ClienteEntity?)null);

        // Act
        var result = await _handler.Handle(new ObterClientePorDocumentoQuery(Documento), CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Cliente.NaoEncontrado", result.Error.Code);
        Assert.Equal("Cliente não encontrado.", result.Error.Message);
    }
}
