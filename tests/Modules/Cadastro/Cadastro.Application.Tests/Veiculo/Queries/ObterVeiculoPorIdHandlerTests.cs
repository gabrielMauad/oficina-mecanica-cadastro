using Cadastro.Application.Gateways;
using Cadastro.Application.Veiculos.Queries.ObterVeiculoPorId;
using Cadastro.Domain.Cliente;
using Cadastro.Domain.Veiculo;
using Moq;
using SharedKernel.Domain;
using VeiculoEntity = Cadastro.Domain.Veiculo.Veiculo;

namespace Cadastro.Application.Tests.Veiculo.Queries;

public class ObterVeiculoPorIdHandlerTests
{
    private readonly Mock<IVeiculoGateway> _gatewayMock;
    private readonly ObterVeiculoPorIdHandler _handler;

    public ObterVeiculoPorIdHandlerTests()
    {
        _gatewayMock = new Mock<IVeiculoGateway>();
        _handler = new ObterVeiculoPorIdHandler(
            _gatewayMock.Object
        );
    }

    [Fact(DisplayName = "Cenário feliz")]
    public async Task Handle_ShouldReturnSucess_WhenQueryIsValid()
    {
        // Arrange
        var query = new ObterVeiculoPorIdQuery(
            Guid.NewGuid()
        );
        var veiculoId = new VeiculoId(query.VeiculoId);
        var clienteId = new ClienteId(Guid.NewGuid());

        VeiculoEntity? veiculo = VeiculoEntity.Criar("ABC1234", "Modelo", "Marca", 2026, clienteId).Value;
        _gatewayMock.Setup(x => x.ObterPorId(veiculoId, It.IsAny<CancellationToken>())).ReturnsAsync(veiculo);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(Error.None, result.Error);

        Assert.NotEqual(Guid.Empty, result.Value.Id.Value);
        Assert.Equal(veiculo.Placa.Numero, result.Value.Placa.Numero);
        Assert.Equal(veiculo.Modelo, result.Value.Modelo);
        Assert.Equal(veiculo.Marca, result.Value.Marca);
        Assert.Equal(veiculo.Ano, result.Value.Ano);
        Assert.Equal(veiculo.ClienteId.Value, result.Value.ClienteId.Value);
        Assert.InRange(result.Value.CadastradoEm, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
        Assert.InRange(result.Value.AtualizadoEm, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
    }

    [Fact(DisplayName = "Erro: Cliente nao encontrado")]
    public async Task Handle_ShouldReturnError_WhenVeiculoNotFound()
    {
        // Arrange
        var query = new ObterVeiculoPorIdQuery(
            Guid.NewGuid()
        );

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Veiculo.NaoEncontrado", result.Error.Code);
        Assert.Equal("Veículo não encontrado.", result.Error.Message);
    }
}

