namespace Cadastro.Contracts.Dtos;

public record VeiculoDto(Guid Id, string Placa, string Modelo, string Marca, int Ano, Guid ClienteId);
