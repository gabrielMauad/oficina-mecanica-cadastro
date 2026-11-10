using Cadastro.Application.Gateways;
using Cadastro.Domain.Servico;
using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Application.Servicos.Commands.AdicionarServico;

public sealed class AdicionarServicoHandler : IRequestHandler<AdicionarServicoCommand, Result<Servico>>
{
    private readonly IServicoGateway _gateway;

    public AdicionarServicoHandler(
        IServicoGateway gateway
    ) => _gateway = gateway;

    public async Task<Result<Servico>> Handle(AdicionarServicoCommand command, CancellationToken cancellationToken)
    {
        if (await _gateway.ExistePorNome(command.Nome, cancellationToken))
            return ServicoErrors.NomeJaExiste;

        Result<Servico> servicoResult = Servico.Criar(command.Nome, command.Descricao, command.Preco);
        if (servicoResult.IsFailure)
            return servicoResult.Error;

        Servico servico = servicoResult.Value;

        await _gateway.Adicionar(servico, cancellationToken);

        return servico;
    }
}

