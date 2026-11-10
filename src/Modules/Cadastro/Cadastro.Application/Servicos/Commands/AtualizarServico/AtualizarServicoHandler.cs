using Cadastro.Application.Gateways;
using Cadastro.Domain.Servico;
using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Application.Servicos.Commands.AtualizarServico;

public sealed class AtualizarServicoHandler : IRequestHandler<AtualizarServicoCommand, Result<Servico>>
{
    private readonly IServicoGateway _gateway;

    public AtualizarServicoHandler(IServicoGateway gateway)
    {
        _gateway = gateway;
    }

    public async Task<Result<Servico>> Handle(AtualizarServicoCommand command, CancellationToken cancellationToken)
    {
        ServicoId servicoId = new(command.ServicoId);
        Servico? servico = await _gateway.ObterPorId(servicoId, cancellationToken);

        if (servico is null)
            return ServicoErrors.NaoEncontrado;

        if (!servico.Ativo)
            return ServicoErrors.JaDesativado;

        bool houveAlteracao = false;

        if (command.Preco is not null && command.Preco != servico.PrecoBase.Valor)
        {
            var result = servico.AtualizarPrecoBase(command.Preco!.Value);
            if (result.IsFailure) return result.Error;
            houveAlteracao = true;
        }

        if (command.Descricao is not null && command.Descricao != servico.Descricao)
        {
            var result = servico.AtualizarDescricao(command.Descricao!);
            if (result.IsFailure) return result.Error;
            houveAlteracao = true;
        }

        if (houveAlteracao)
            await _gateway.Atualizar(servico, cancellationToken);

        return servico;
    }
}

