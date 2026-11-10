using Cadastro.Adapters.Models.Request;
using Cadastro.Adapters.Models.ViewModels;
using Cadastro.Adapters.Presenters;
using Cadastro.Application.Veiculos.Commands.CadastrarVeiculo;
using Cadastro.Application.Veiculos.Queries.ListarVeiculos;
using Cadastro.Application.Veiculos.Queries.ListarVeiculosPorCliente;
using Cadastro.Application.Veiculos.Queries.ObterVeiculoPorId;
using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Adapters.Controllers;

public sealed class VeiculoController
{
    private readonly ISender _sender;

    public VeiculoController(ISender sender) => _sender = sender;

    public async Task<Result<CadastrarVeiculoViewModel>> Criar(CadastrarVeiculoRequest request)
    {
        var command = new CadastrarVeiculoCommand(request.Placa, request.Modelo, request.Marca, request.Ano, request.ClienteId);
        var result = await _sender.Send(command);
        if (result.IsFailure) return result.Error;
        return VeiculoPresenter.PresentCadastrar(result.Value);
    }

    public async Task<Result<List<VeiculoListItemViewModel>>> Listar()
    {
        var result = await _sender.Send(new ListarVeiculosQuery());
        if (result.IsFailure) return result.Error;
        return VeiculoPresenter.PresentListar(result.Value);
    }

    public async Task<Result<ObterVeiculoPorIdViewModel>> ObterPorId(Guid id)
    {
        var result = await _sender.Send(new ObterVeiculoPorIdQuery(id));
        if (result.IsFailure) return result.Error;
        return VeiculoPresenter.PresentObterPorId(result.Value);
    }

    public async Task<Result<VeiculosPorClienteViewModel>> ObterVeiculosPorClienteId(Guid clienteId)
    {
        var result = await _sender.Send(new ListarVeiculosPorClienteQuery(clienteId));
        if (result.IsFailure) return result.Error;
        return VeiculoPresenter.PresentListarPorCliente(result.Value);
    }
}
