using Cadastro.Application.Gateways;
using Cadastro.Domain.Servico;
using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Application.Servicos.Queries.ObterServicoPorId;

public sealed class ObterServicoPorIdHandler : IRequestHandler<ObterServicoPorIdQuery, Result<Servico>>
{
    private readonly IServicoGateway _gateway;

    public ObterServicoPorIdHandler(IServicoGateway gateway) => _gateway = gateway;

    public async Task<Result<Servico>> Handle(ObterServicoPorIdQuery request, CancellationToken cancellationToken)
    {
        ServicoId servicoId = new(request.ServicoId);
        Servico? servico = await _gateway.ObterPorId(servicoId, cancellationToken);
        if (servico is null)
            return ServicoErrors.NaoEncontrado;

        return servico;
    }
}

