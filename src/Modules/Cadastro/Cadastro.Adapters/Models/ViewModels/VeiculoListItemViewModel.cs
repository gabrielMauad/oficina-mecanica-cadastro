namespace Cadastro.Adapters.Models.ViewModels;

public sealed record VeiculoListItemViewModel(
    Guid Id,
    string Placa,
    string Modelo,
    string Marca,
    int Ano,
    Guid ClienteId,
    DateTime CadastradoEm,
    DateTime AtualizadoEm);
