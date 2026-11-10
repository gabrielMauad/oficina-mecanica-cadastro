namespace Cadastro.Application.Veiculos.Queries.ListarVeiculos;

public sealed record VeiculoListItem(
    Guid Id,
    string Placa,
    string Modelo,
    string Marca,
    int Ano,
    Guid ClienteId,
    DateTime CadastradoEm,
    DateTime AtualizadoEm
);
