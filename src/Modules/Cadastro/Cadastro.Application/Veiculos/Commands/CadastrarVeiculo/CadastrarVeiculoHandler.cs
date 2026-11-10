using Cadastro.Application.Gateways;
using Cadastro.Domain.Cliente;
using Cadastro.Domain.Veiculo;
using MediatR;
using SharedKernel.Domain;

namespace Cadastro.Application.Veiculos.Commands.CadastrarVeiculo;

public sealed class CadastrarVeiculoHandler
    : IRequestHandler<CadastrarVeiculoCommand, Result<Veiculo>>
{
    private readonly IVeiculoGateway _gateway;
    private readonly IClienteGateway _clienteGateway;

    public CadastrarVeiculoHandler(
        IVeiculoGateway gateway,
        IClienteGateway clienteGateway)
    {
        _gateway = gateway;
        _clienteGateway = clienteGateway;
    }

    public async Task<Result<Veiculo>> Handle(CadastrarVeiculoCommand command, CancellationToken cancellationToken)
    {
        var placaNormalizada = command.Placa.ToUpperInvariant().Replace("-", "");

        if (await _gateway.ExistePorPlaca(placaNormalizada, cancellationToken))
            return VeiculoErrors.PlacaJaExiste;

        ClienteId clienteId = new(command.ClienteId);
        Cliente? cliente = await _clienteGateway.ObterPorId(clienteId, cancellationToken);

        if (cliente is null)
            return VeiculoErrors.ClienteNaoEncontrado;

        if (!cliente.Ativo)
            return VeiculoErrors.ClienteInativo;

        Result<Veiculo> veiculoResult = Veiculo.Criar(command.Placa, command.Modelo, command.Marca, command.Ano, clienteId);

        if (veiculoResult.IsFailure)
            return veiculoResult.Error;

        Veiculo veiculo = veiculoResult.Value;

        await _gateway.Adicionar(veiculo, cancellationToken);

        return veiculo;
    }
}
