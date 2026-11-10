using Cadastro.Adapters.DataSources.Records;
using Cadastro.Domain.Cliente;

namespace Cadastro.Adapters.DataSources.Mappers;

internal static class ClienteMapper
{
    public static ClienteRecord ToRecord(Cliente cliente) => new()
    {
        Id = cliente.Id.Value,
        Nome = cliente.Nome,
        Documento = cliente.Documento.Numero,
        Email = cliente.Email,
        Telefone = cliente.Telefone,
        Ativo = cliente.Ativo,
        CadastradoEm = cliente.CadastradoEm,
        AtualizadoEm = cliente.AtualizadoEm
    };

    public static Cliente ToDomain(ClienteRecord record) => Cliente.Reconstituir(
        new ClienteId(record.Id),
        record.Nome,
        Documento.Reconstituir(record.Documento),
        record.Email,
        record.Telefone,
        record.Ativo,
        record.CadastradoEm,
        record.AtualizadoEm);
}
