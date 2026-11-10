using Cadastro.Domain.Servico;
using SharedKernel.Application;

namespace Cadastro.Application.Servicos.Commands.DesativarServico;

public sealed record DesativarServicoCommand(
    Guid ServicoId
) : ICommand<Servico>;

