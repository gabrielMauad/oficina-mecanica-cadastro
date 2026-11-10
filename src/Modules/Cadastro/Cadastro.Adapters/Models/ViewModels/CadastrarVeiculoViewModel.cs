namespace Cadastro.Adapters.Models.ViewModels;

public sealed record CadastrarVeiculoViewModel(
    Guid VeiculoId,
    string Placa,
    string Modelo,
    string Marca,
    int Ano,
    Guid ClienteId,
    DateTime CadastradoEm,
    DateTime AtualizadoEm);
