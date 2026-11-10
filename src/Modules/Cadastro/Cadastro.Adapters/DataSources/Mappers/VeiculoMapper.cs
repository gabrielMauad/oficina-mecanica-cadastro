using Cadastro.Adapters.DataSources.Records;
using Cadastro.Domain.Cliente;
using Cadastro.Domain.Veiculo;

namespace Cadastro.Adapters.DataSources.Mappers;

internal static class VeiculoMapper
{
    public static VeiculoRecord ToRecord(Veiculo veiculo) => new()
    {
        Id = veiculo.Id.Value,
        Placa = veiculo.Placa.Numero,
        Modelo = veiculo.Modelo,
        Marca = veiculo.Marca,
        Ano = veiculo.Ano,
        ClienteId = veiculo.ClienteId.Value,
        CadastradoEm = veiculo.CadastradoEm,
        AtualizadoEm = veiculo.AtualizadoEm
    };

    public static Veiculo ToDomain(VeiculoRecord record) => Veiculo.Reconstituir(
        new VeiculoId(record.Id),
        Placa.Reconstituir(record.Placa),
        record.Modelo,
        record.Marca,
        record.Ano,
        new ClienteId(record.ClienteId),
        record.CadastradoEm,
        record.AtualizadoEm);
}
