namespace Cadastro.Contracts.Dtos;

public record ClienteDto(Guid Id, string Nome, string Documento, string Email, bool Ativo);
