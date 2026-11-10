using Cadastro.Adapters.Models.Request;
using Cadastro.Adapters.Models.ViewModels;
using Cadastro.Adapters.Presenters;
using Cadastro.Application.Servicos.Commands.AdicionarServico;
using Cadastro.Application.Servicos.Commands.AtualizarServico;
using Cadastro.Application.Servicos.Commands.DesativarServico;
using Cadastro.Application.Servicos.Queries.ListarServicos;
using Cadastro.Application.Servicos.Queries.ObterServicoPorId;
using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Adapters.Controllers;

public sealed class ServicoController
{
    private readonly ISender _sender;

    public ServicoController(ISender sender) => _sender = sender;

    public async Task<Result<AdicionarServicoViewModel>> Criar(AdicionarServicoRequest request)
    {
        var command = new AdicionarServicoCommand(request.Nome, request.Descricao, request.Preco);
        var result = await _sender.Send(command);
        if (result.IsFailure) return result.Error;
        return ServicoPresenter.PresentAdicionar(result.Value);
    }

    public async Task<Result<List<ServicoListItemViewModel>>> Listar()
    {
        var result = await _sender.Send(new ListarServicosQuery());
        if (result.IsFailure) return result.Error;
        return ServicoPresenter.PresentListar(result.Value);
    }

    public async Task<Result<ObterServicoPorIdViewModel>> ObterPorId(Guid id)
    {
        var result = await _sender.Send(new ObterServicoPorIdQuery(id));
        if (result.IsFailure) return result.Error;
        return ServicoPresenter.PresentObterPorId(result.Value);
    }

    public async Task<Result<AtualizarServicoViewModel>> AtualizarDescricao(Guid id, AtualizarDescricaoRequest request)
    {
        var command = new AtualizarServicoCommand(id, request.Descricao, null);
        var result = await _sender.Send(command);
        if (result.IsFailure) return result.Error;
        return ServicoPresenter.PresentAtualizar(result.Value);
    }

    public async Task<Result<AtualizarServicoViewModel>> AtualizarPreco(Guid id, AtualizarPrecoRequest request)
    {
        var command = new AtualizarServicoCommand(id, null, request.Preco);
        var result = await _sender.Send(command);
        if (result.IsFailure) return result.Error;
        return ServicoPresenter.PresentAtualizar(result.Value);
    }

    public async Task<Result<DesativarServicoViewModel>> Desativar(Guid id)
    {
        var command = new DesativarServicoCommand(id);
        var result = await _sender.Send(command);
        if (result.IsFailure) return result.Error;
        return ServicoPresenter.PresentDesativar(result.Value);
    }
}
