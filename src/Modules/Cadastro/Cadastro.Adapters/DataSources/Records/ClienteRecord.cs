namespace Cadastro.Adapters.DataSources.Records;

public sealed class ClienteRecord
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = "";
    public string Documento { get; set; } = "";
    public string Email { get; set; } = "";
    public string Telefone { get; set; } = "";
    public bool Ativo { get; set; }
    public DateTime CadastradoEm { get; set; }
    public DateTime AtualizadoEm { get; set; }
}
