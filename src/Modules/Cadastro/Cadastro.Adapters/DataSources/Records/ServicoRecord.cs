namespace Cadastro.Adapters.DataSources.Records;

public sealed class ServicoRecord
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = "";
    public string? Descricao { get; set; }
    public decimal PrecoBase { get; set; }
    public bool Ativo { get; set; }
    public DateTime CadastradoEm { get; set; }
    public DateTime AtualizadoEm { get; set; }
}
