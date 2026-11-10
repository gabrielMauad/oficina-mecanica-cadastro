using Cadastro.Domain.Servico;

namespace Cadastro.Application.Gateways;

public interface IServicoGateway
{
    Task Adicionar(Servico servico, CancellationToken ct = default);
    Task<Servico?> ObterPorId(ServicoId id, CancellationToken ct = default);
    Task<bool> ExistePorNome(string nome, CancellationToken ct = default);
    Task Atualizar(Servico servico, CancellationToken ct = default);
}
