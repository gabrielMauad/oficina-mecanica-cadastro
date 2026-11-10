namespace Cadastro.Adapters.Models.ViewModels;

public sealed record ObterVeiculoPorIdViewModel(
    Guid Id,
    string Placa,
    string Modelo,
    string Marca,
    int Ano,
    Guid ClienteId,
    DateTime CadastradoEm,
    DateTime AtualizadoEm);
