using Cadastro.Adapters.Models.ViewModels;
using Cadastro.Application.Clientes.Queries.ListarClientes;
using Cadastro.Domain.Cliente;

namespace Cadastro.Adapters.Presenters;

public static class ClientePresenter
{
    public static CadastrarClienteViewModel PresentCadastrar(Cliente entity) =>
        new(
            entity.Id.Value,
            entity.Nome,
            entity.Documento.Numero,
            entity.Email,
            entity.Telefone,
            entity.Ativo,
            entity.CadastradoEm,
            entity.AtualizadoEm);

    public static AtualizarClienteViewModel PresentAtualizar(Cliente entity) =>
        new(
            entity.Id.Value,
            entity.Nome,
            entity.Telefone,
            entity.Ativo,
            entity.CadastradoEm,
            entity.AtualizadoEm);

    public static DesativarClienteViewModel PresentDesativar(Cliente entity) =>
        new(
            entity.Id.Value,
            entity.Nome,
            entity.Ativo,
            entity.CadastradoEm,
            entity.AtualizadoEm);

    public static ObterClientePorIdViewModel PresentObterPorId(Cliente entity) =>
        new(
            entity.Id.Value,
            entity.Nome,
            entity.Documento.Numero,
            entity.Email,
            entity.Telefone,
            entity.Ativo,
            entity.CadastradoEm,
            entity.AtualizadoEm);

    public static List<ClienteListItemViewModel> PresentListar(List<ClienteListItem> items) =>
        items.Select(i => new ClienteListItemViewModel(
            i.Id,
            i.Nome,
            i.Documento,
            i.Email,
            i.Telefone,
            i.Ativo,
            i.CadastradoEm,
            i.AtualizadoEm)).ToList();
}
