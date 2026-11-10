namespace Cadastro.Domain.Servico;

public sealed record ServicoId(Guid Value)
{
    public static ServicoId Novo() => new(Guid.NewGuid());
}

