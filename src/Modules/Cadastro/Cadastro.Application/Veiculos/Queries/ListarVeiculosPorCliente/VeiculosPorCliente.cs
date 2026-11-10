namespace Cadastro.Application.Veiculos.Queries.ListarVeiculosPorCliente;

public sealed record VeiculosPorCliente(
    Guid ClienteId,
    string NomeCliente,
    IReadOnlyList<VeiculoDoCliente> Veiculos
);

public sealed record VeiculoDoCliente(
    Guid VeiculoId,
    string Placa,
    string Modelo,
    string Marca,
    int Ano,
    DateTime CadastradoEm,
    DateTime AtualizadoEm
);

