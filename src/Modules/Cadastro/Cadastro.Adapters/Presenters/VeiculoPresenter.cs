using Cadastro.Adapters.Models.ViewModels;
using Cadastro.Application.Veiculos.Queries.ListarVeiculos;
using Cadastro.Application.Veiculos.Queries.ListarVeiculosPorCliente;
using Cadastro.Domain.Veiculo;

namespace Cadastro.Adapters.Presenters;

public static class VeiculoPresenter
{
    public static CadastrarVeiculoViewModel PresentCadastrar(Veiculo entity) =>
        new(
            entity.Id.Value,
            entity.Placa.Numero,
            entity.Modelo,
            entity.Marca,
            entity.Ano,
            entity.ClienteId.Value,
            entity.CadastradoEm,
            entity.AtualizadoEm);

    public static ObterVeiculoPorIdViewModel PresentObterPorId(Veiculo entity) =>
        new(
            entity.Id.Value,
            entity.Placa.Numero,
            entity.Modelo,
            entity.Marca,
            entity.Ano,
            entity.ClienteId.Value,
            entity.CadastradoEm,
            entity.AtualizadoEm);

    public static List<VeiculoListItemViewModel> PresentListar(List<VeiculoListItem> items) =>
        items.Select(i => new VeiculoListItemViewModel(
            i.Id,
            i.Placa,
            i.Modelo,
            i.Marca,
            i.Ano,
            i.ClienteId,
            i.CadastradoEm,
            i.AtualizadoEm)).ToList();

    public static VeiculosPorClienteViewModel PresentListarPorCliente(VeiculosPorCliente readModel) =>
        new(
            readModel.ClienteId,
            readModel.NomeCliente,
            readModel.Veiculos.Select(v => new VeiculoDoClienteViewModel(
                v.VeiculoId,
                v.Placa,
                v.Modelo,
                v.Marca,
                v.Ano,
                v.CadastradoEm,
                v.AtualizadoEm)).ToList());
}
