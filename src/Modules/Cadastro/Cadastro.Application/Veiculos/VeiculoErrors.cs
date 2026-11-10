using SharedKernel.Domain;

namespace Cadastro.Application.Veiculos;

internal static class VeiculoErrors
{
    public static readonly Error PlacaJaExiste = Error.Conflict(
        "Veiculo.PlacaJaExiste",
        "Já existe um veículo cadastrado com esta placa.");

    public static readonly Error NaoEncontrado = Error.NotFound(
        "Veiculo.NaoEncontrado",
        "Veículo não encontrado.");

    public static readonly Error ClienteNaoEncontrado = Error.NotFound(
        "Veiculo.ClienteNaoEncontrado",
        "Cliente não encontrado.");

    public static readonly Error ClienteInativo = Error.Conflict(
        "Veiculo.ClienteInativo",
        "Não é possível cadastrar veículo para um cliente inativo.");
}
