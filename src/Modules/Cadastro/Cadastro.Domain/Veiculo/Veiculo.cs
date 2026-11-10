using Cadastro.Domain.Cliente;
using SharedKernel.Domain;

namespace Cadastro.Domain.Veiculo;

public sealed class Veiculo : AggregateRoot<VeiculoId>
{
    public Placa Placa { get; private set; }
    public string Modelo { get; private set; }
    public string Marca { get; private set; }
    public int Ano { get; private set; }
    public ClienteId ClienteId { get; private set; }
    public DateTime CadastradoEm { get; private set; }
    public DateTime AtualizadoEm { get; private set; }

    private Veiculo(VeiculoId id, Placa placa, string modelo, string marca, int ano, ClienteId clienteId)
       : base(id)
    {
        Placa = placa;
        Modelo = modelo;
        Marca = marca;
        Ano = ano;
        ClienteId = clienteId;
        CadastradoEm = DateTime.UtcNow;
        AtualizadoEm = DateTime.UtcNow;
    }

    private Veiculo(VeiculoId id, Placa placa, string modelo, string marca, int ano, ClienteId clienteId,
        DateTime cadastradoEm, DateTime atualizadoEm) : base(id)
    {
        Placa = placa;
        Modelo = modelo;
        Marca = marca;
        Ano = ano;
        ClienteId = clienteId;
        CadastradoEm = cadastradoEm;
        AtualizadoEm = atualizadoEm;
    }

    public static Veiculo Reconstituir(VeiculoId id, Placa placa, string modelo, string marca, int ano,
        ClienteId clienteId, DateTime cadastradoEm, DateTime atualizadoEm) =>
        new(id, placa, modelo, marca, ano, clienteId, cadastradoEm, atualizadoEm);

    public static Result<Veiculo> Criar(string numPlaca, string modelo, string marca, int ano, ClienteId clienteId)
    {
        if (string.IsNullOrWhiteSpace(modelo))
            return Error.Validation("Veiculo.ModeloVazio", "Modelo é obrigatório.");
        if (string.IsNullOrWhiteSpace(marca))
            return Error.Validation("Veiculo.MarcaVazia", "Marca é obrigatória.");
        if (ano < 1886 || ano > DateTime.UtcNow.Year + 1)
            return Error.Validation("Veiculo.AnoInvalido", "Ano inválido.");
        if (clienteId is null)
            return Error.Validation("Veiculo.ClienteIdVazio", "Id do cliente é obrigatório.");

        Result<Placa> placaResult = Placa.Criar(numPlaca);
        if (placaResult.IsFailure) return placaResult.Error;
        Placa placa = placaResult.Value;

        var veiculo = new Veiculo(VeiculoId.Novo(), placa, modelo, marca, ano, clienteId);

        return veiculo;
    }
}
