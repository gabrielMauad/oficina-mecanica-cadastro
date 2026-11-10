using Cadastro.Adapters.Models.Request;
using Cadastro.Adapters.Models.ViewModels;
using Cadastro.Adapters.Presenters;
using Cadastro.Application.Clientes.Commands.AtualizarCliente;
using Cadastro.Application.Clientes.Commands.CadastrarCliente;
using Cadastro.Application.Clientes.Commands.DesativarCliente;
using Cadastro.Application.Clientes.Queries.ListarClientes;
using Cadastro.Application.Clientes.Queries.ObterClientePorDocumento;
using Cadastro.Application.Clientes.Queries.ObterClientePorId;
using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Adapters.Controllers;

public sealed class ClienteController
{
    private readonly ISender _sender;

    public ClienteController(ISender sender) => _sender = sender;

    public async Task<Result<CadastrarClienteViewModel>> Criar(CadastrarClienteRequest request)
    {
        var command = new CadastrarClienteCommand(
            request.Nome,
            request.Documento,
            request.Email,
            request.Telefone,
            request.PessoaFisica);
        var result = await _sender.Send(command);
        if (result.IsFailure) return result.Error;
        return ClientePresenter.PresentCadastrar(result.Value);
    }

    public async Task<Result<List<ClienteListItemViewModel>>> Listar()
    {
        var result = await _sender.Send(new ListarClientesQuery());
        if (result.IsFailure) return result.Error;
        return ClientePresenter.PresentListar(result.Value);
    }

    public async Task<Result<ObterClientePorIdViewModel>> ObterPorId(Guid id)
    {
        var result = await _sender.Send(new ObterClientePorIdQuery(id));
        if (result.IsFailure) return result.Error;
        return ClientePresenter.PresentObterPorId(result.Value);
    }

    public async Task<Result<ObterClientePorIdViewModel>> ObterPorDocumento(string documento)
    {
        var result = await _sender.Send(new ObterClientePorDocumentoQuery(documento));
        if (result.IsFailure) return result.Error;
        return ClientePresenter.PresentObterPorId(result.Value);
    }

    public async Task<Result<AtualizarClienteViewModel>> AtualizarNome(Guid id, AtualizarNomeRequest request)
    {
        var command = new AtualizarClienteCommand(id, request.Nome, null);
        var result = await _sender.Send(command);
        if (result.IsFailure) return result.Error;
        return ClientePresenter.PresentAtualizar(result.Value);
    }

    public async Task<Result<AtualizarClienteViewModel>> AtualizarTelefone(Guid id, AtualizarTelefoneRequest request)
    {
        var command = new AtualizarClienteCommand(id, null, request.Telefone);
        var result = await _sender.Send(command);
        if (result.IsFailure) return result.Error;
        return ClientePresenter.PresentAtualizar(result.Value);
    }

    public async Task<Result<DesativarClienteViewModel>> Desativar(Guid id)
    {
        var command = new DesativarClienteCommand(id);
        var result = await _sender.Send(command);
        if (result.IsFailure) return result.Error;
        return ClientePresenter.PresentDesativar(result.Value);
    }
}
