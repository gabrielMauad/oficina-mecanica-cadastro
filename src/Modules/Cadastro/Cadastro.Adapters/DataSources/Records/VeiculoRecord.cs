namespace Cadastro.Adapters.DataSources.Records;

public sealed class VeiculoRecord
{
    public Guid Id { get; set; }
    public string Placa { get; set; } = "";
    public string Modelo { get; set; } = "";
    public string Marca { get; set; } = "";
    public int Ano { get; set; }
    public Guid ClienteId { get; set; }
    public DateTime CadastradoEm { get; set; }
    public DateTime AtualizadoEm { get; set; }
}
