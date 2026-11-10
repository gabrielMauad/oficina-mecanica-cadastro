namespace Cadastro.Domain.Veiculo;

public sealed record VeiculoId(Guid Value)
{
    public static VeiculoId Novo() => new(Guid.NewGuid());
}