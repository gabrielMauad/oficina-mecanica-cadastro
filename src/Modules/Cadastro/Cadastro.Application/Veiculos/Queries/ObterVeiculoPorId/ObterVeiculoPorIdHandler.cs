using Cadastro.Application.Gateways;
using Cadastro.Domain.Veiculo;
using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Application.Veiculos.Queries.ObterVeiculoPorId;

public sealed class ObterVeiculoPorIdHandler
    : IRequestHandler<ObterVeiculoPorIdQuery, Result<Veiculo>>
{
    private readonly IVeiculoGateway _gateway;

    public ObterVeiculoPorIdHandler(IVeiculoGateway gateway) => _gateway = gateway;

    public async Task<Result<Veiculo>> Handle(ObterVeiculoPorIdQuery request, CancellationToken cancellationToken)
    {
        VeiculoId veiculoId = new(request.VeiculoId);
        var veiculo = await _gateway.ObterPorId(veiculoId, cancellationToken);
        if (veiculo is null)
            return VeiculoErrors.NaoEncontrado;
        return veiculo;
    }

}

