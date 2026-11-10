using Cadastro.Domain.Veiculo;
using SharedKernel.Application;

namespace Cadastro.Application.Veiculos.Commands.CadastrarVeiculo;

public sealed record CadastrarVeiculoCommand(
    string Placa,
    string Modelo,
    string Marca,
    int Ano,
    Guid ClienteId
) : ICommand<Veiculo>;
