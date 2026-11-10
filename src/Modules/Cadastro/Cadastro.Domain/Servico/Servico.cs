using SharedKernel.Domain;

namespace Cadastro.Domain.Servico;

public sealed class Servico : AggregateRoot<ServicoId>
{
    public string Nome { get; private set; }
    public string? Descricao { get; private set; }
    public Dinheiro PrecoBase { get; private set; }
    public bool Ativo { get; private set; }
    public DateTime CadastradoEm { get; private set; }
    public DateTime AtualizadoEm { get; private set; }

    private Servico(ServicoId id, string nome, string? descricao, Dinheiro precoBase)
       : base(id)
    {
        Nome = nome;
        Descricao = descricao;
        PrecoBase = precoBase;
        Ativo = true;
        CadastradoEm = DateTime.UtcNow;
        AtualizadoEm = DateTime.UtcNow;
    }

    private Servico(ServicoId id, string nome, string? descricao, Dinheiro precoBase,
        bool ativo, DateTime cadastradoEm, DateTime atualizadoEm) : base(id)
    {
        Nome = nome;
        Descricao = descricao;
        PrecoBase = precoBase;
        Ativo = ativo;
        CadastradoEm = cadastradoEm;
        AtualizadoEm = atualizadoEm;
    }

    public static Servico Reconstituir(ServicoId id, string nome, string? descricao, Dinheiro precoBase,
        bool ativo, DateTime cadastradoEm, DateTime atualizadoEm) =>
        new(id, nome, descricao, precoBase, ativo, cadastradoEm, atualizadoEm);

    public static Result<Servico> Criar(string nome, string? descricao, decimal preco)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Error.Validation("Servico.NomeVazio", "Nome é obrigatório.");
        Result<Dinheiro> precoBaseResult = Dinheiro.Criar(preco);
        if (precoBaseResult.IsFailure)
            return precoBaseResult.Error;
        Dinheiro precoBase = precoBaseResult.Value;

        var servico = new Servico(ServicoId.Novo(), nome, descricao, precoBase);

        return servico;
    }

    public Result<Servico> AtualizarDescricao(string novaDescricao)
    {
        Descricao = novaDescricao;
        AtualizadoEm = DateTime.UtcNow;
        return this;
    }

    public Result<Servico> AtualizarPrecoBase(decimal novoPreco)
    {
        Result<Dinheiro> precoBaseResult = Dinheiro.Criar(novoPreco);
        if (precoBaseResult.IsFailure)
            return precoBaseResult.Error;
        PrecoBase = precoBaseResult.Value;
        AtualizadoEm = DateTime.UtcNow;
        return this;
    }

    public void Desativar()
    {
        Ativo = false;
        AtualizadoEm = DateTime.UtcNow;
    }
}
