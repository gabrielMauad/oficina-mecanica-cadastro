using Cadastro.Adapters.DataSources.Records;
using Cadastro.Domain.Servico;

namespace Cadastro.Adapters.DataSources.Mappers;

internal static class ServicoMapper
{
    public static ServicoRecord ToRecord(Servico servico) => new()
    {
        Id = servico.Id.Value,
        Nome = servico.Nome,
        Descricao = servico.Descricao,
        PrecoBase = servico.PrecoBase.Valor,
        Ativo = servico.Ativo,
        CadastradoEm = servico.CadastradoEm,
        AtualizadoEm = servico.AtualizadoEm
    };

    public static Servico ToDomain(ServicoRecord record) => Servico.Reconstituir(
        new ServicoId(record.Id),
        record.Nome,
        record.Descricao,
        Dinheiro.Reconstituir(record.PrecoBase),
        record.Ativo,
        record.CadastradoEm,
        record.AtualizadoEm);
}
