namespace Cadastro.Adapters.Models.Request;

public sealed record CadastrarVeiculoRequest(
    string Placa,
    string Modelo,
    string Marca,
    int Ano,
    Guid ClienteId);
