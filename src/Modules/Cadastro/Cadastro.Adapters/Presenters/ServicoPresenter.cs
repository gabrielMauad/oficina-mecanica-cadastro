using Cadastro.Adapters.Models.ViewModels;
using Cadastro.Application.Servicos.Queries.ListarServicos;
using Cadastro.Domain.Servico;

namespace Cadastro.Adapters.Presenters;

public static class ServicoPresenter
{
    public static AdicionarServicoViewModel PresentAdicionar(Servico entity) =>
        new(
            entity.Id.Value,
            entity.Nome,
            entity.Descricao,
            entity.PrecoBase.Valor,
            entity.Ativo,
            entity.CadastradoEm,
            entity.AtualizadoEm);

    public static AtualizarServicoViewModel PresentAtualizar(Servico entity) =>
        new(
            entity.Id.Value,
            entity.Descricao,
            entity.PrecoBase.Valor,
            entity.Ativo,
            entity.CadastradoEm,
            entity.AtualizadoEm);

    public static DesativarServicoViewModel PresentDesativar(Servico entity) =>
        new(
            entity.Id.Value,
            entity.Nome,
            entity.Ativo);

    public static ObterServicoPorIdViewModel PresentObterPorId(Servico entity) =>
        new(
            entity.Id.Value,
            entity.Nome,
            entity.Descricao,
            entity.PrecoBase.Valor,
            entity.Ativo,
            entity.CadastradoEm,
            entity.AtualizadoEm);

    public static List<ServicoListItemViewModel> PresentListar(List<ServicoListItem> items) =>
        items.Select(i => new ServicoListItemViewModel(
            i.Id,
            i.Nome,
            i.Descricao,
            i.Preco,
            i.Ativo,
            i.CadastradoEm,
            i.AtualizadoEm)).ToList();
}
