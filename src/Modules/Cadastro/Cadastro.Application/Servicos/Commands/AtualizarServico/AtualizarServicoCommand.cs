using Cadastro.Domain.Servico;
using SharedKernel.Application;

namespace Cadastro.Application.Servicos.Commands.AtualizarServico;

public sealed record AtualizarServicoCommand(
    Guid ServicoId,
    string? Descricao,
    decimal? Preco
) : ICommand<Servico>;
