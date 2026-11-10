using Cadastro.Application.Gateways;
using Cadastro.Domain.Servico;
using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Application.Servicos.Commands.DesativarServico;

public sealed class DesativarServicoHandler : IRequestHandler<DesativarServicoCommand, Result<Servico>>
{
    private readonly IServicoGateway _gateway;

    public DesativarServicoHandler(IServicoGateway gateway) => _gateway = gateway;

    public async Task<Result<Servico>> Handle(DesativarServicoCommand command, CancellationToken cancellationToken)
    {
        ServicoId servicoId = new(command.ServicoId);
        Servico? servico = await _gateway.ObterPorId(servicoId, cancellationToken);
        if (servico is null)
            return ServicoErrors.NaoEncontrado;
        if (!servico.Ativo)
            return ServicoErrors.JaDesativado;
        servico.Desativar();
        await _gateway.Atualizar(servico, cancellationToken);

        return servico;
    }
}

