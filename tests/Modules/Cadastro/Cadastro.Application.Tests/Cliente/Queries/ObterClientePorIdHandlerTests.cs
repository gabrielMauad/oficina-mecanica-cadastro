using Cadastro.Application.Clientes.Queries.ObterClientePorId;
using Cadastro.Application.Gateways;
using Cadastro.Domain.Cliente;
using Moq;
using SharedKernel.Domain;
using ClienteEntity = Cadastro.Domain.Cliente.Cliente;

namespace Cadastro.Application.Tests.Cliente.Queries;

public class ObterClientePorIdHandlerTests
{
    private readonly Mock<IClienteGateway> _gatewayMock;
    private readonly ObterClientePorIdHandler _handler;

    public ObterClientePorIdHandlerTests()
    {
        _gatewayMock = new Mock<IClienteGateway>();
        _handler = new ObterClientePorIdHandler(
            _gatewayMock.Object
        );
    }

    [Fact(DisplayName = "Cenário feliz")]
    public async Task Handle_ShouldReturnSucess_WhenQueryIsValid()
    {
        // Arrange
        var query = new ObterClientePorIdQuery(
            Guid.NewGuid()
        );
        var clienteId = new ClienteId(query.ClienteId);
        ClienteEntity? cliente = ClienteEntity.Criar("nome", "01404238000", "email@exemplo.com", "11999999999", true).Value;

        _gatewayMock.Setup(x => x.ObterPorId(clienteId, It.IsAny<CancellationToken>())).ReturnsAsync(cliente);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(Error.None, result.Error);

        Assert.NotEqual(Guid.Empty, result.Value.Id.Value);
        Assert.Equal(cliente.Nome, result.Value.Nome);
        Assert.Equal(cliente.Documento.Numero, result.Value.Documento.Numero);
        Assert.Equal(cliente.Email, result.Value.Email);
        Assert.Equal(cliente.Telefone, result.Value.Telefone);
        Assert.True(result.Value.Ativo);
        Assert.InRange(result.Value.CadastradoEm, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
        Assert.InRange(result.Value.AtualizadoEm, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
    }

    [Fact(DisplayName = "Erro: Cliente nao encontrado")]
    public async Task Handle_ShouldReturnError_WhenClienteNotFound()
    {
        // Arrange
        var query = new ObterClientePorIdQuery(
            Guid.NewGuid()
        );

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Cliente.NaoEncontrado", result.Error.Code);
        Assert.Equal("Cliente não encontrado.", result.Error.Message);
    }
}
