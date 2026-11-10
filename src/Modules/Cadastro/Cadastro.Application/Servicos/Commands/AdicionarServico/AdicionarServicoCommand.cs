using Cadastro.Domain.Servico;
using SharedKernel.Application;

namespace Cadastro.Application.Servicos.Commands.AdicionarServico;

public sealed record AdicionarServicoCommand(
    string Nome,
    string? Descricao,
    decimal Preco
) : ICommand<Servico>;
