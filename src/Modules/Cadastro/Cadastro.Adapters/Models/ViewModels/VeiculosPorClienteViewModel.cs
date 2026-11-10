namespace Cadastro.Adapters.Models.ViewModels;

public sealed record VeiculosPorClienteViewModel(
    Guid ClienteId,
    string NomeCliente,
    IReadOnlyList<VeiculoDoClienteViewModel> Veiculos);

public sealed record VeiculoDoClienteViewModel(
    Guid VeiculoId,
    string Placa,
    string Modelo,
    string Marca,
    int Ano,
    DateTime CadastradoEm,
    DateTime AtualizadoEm);
