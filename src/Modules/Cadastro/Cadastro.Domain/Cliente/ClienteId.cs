namespace Cadastro.Domain.Cliente;

public sealed record ClienteId(Guid Value)
{
    public static ClienteId Novo() => new(Guid.NewGuid());
}
