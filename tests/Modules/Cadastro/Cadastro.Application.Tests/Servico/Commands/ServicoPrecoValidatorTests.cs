using Cadastro.Application.Servicos.Commands.AdicionarServico;
using Cadastro.Application.Servicos.Commands.AtualizarServico;

namespace Cadastro.Application.Tests.Servico.Commands;

/// <summary>
/// O preço é persistido em <c>numeric(10,2)</c>: no máximo 8 dígitos inteiros e 2 casas decimais.
/// </summary>
public class ServicoPrecoValidatorTests
{
    private readonly AdicionarServicoValidator _adicionar = new();
    private readonly AtualizarServicoValidator _atualizar = new();

    public static TheoryData<decimal> PrecosValidos => new() { 0m, 150m, 150.5m, 99999999.99m, 10.100m };

    public static TheoryData<decimal> PrecosForaDaColuna => new()
    {
        100000000m, 100000000.00m, 10.999m, 0.001m, decimal.MaxValue
    };

    [Theory(DisplayName = "Adicionar: preço dentro de numeric(10,2) é válido")]
    [MemberData(nameof(PrecosValidos))]
    public void Adicionar_ShouldBeValid_WhenPrecoFitsColumn(decimal preco)
    {
        var result = _adicionar.Validate(new AdicionarServicoCommand("Troca de óleo", null, preco));

        Assert.True(result.IsValid);
    }

    [Theory(DisplayName = "Adicionar: preço fora de numeric(10,2) é inválido")]
    [MemberData(nameof(PrecosForaDaColuna))]
    public void Adicionar_ShouldBeInvalid_WhenPrecoExceedsColumn(decimal preco)
    {
        var result = _adicionar.Validate(new AdicionarServicoCommand("Troca de óleo", null, preco));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(AdicionarServicoCommand.Preco));
    }

    [Theory(DisplayName = "Atualizar: preço dentro de numeric(10,2) é válido")]
    [MemberData(nameof(PrecosValidos))]
    public void Atualizar_ShouldBeValid_WhenPrecoFitsColumn(decimal preco)
    {
        var result = _atualizar.Validate(new AtualizarServicoCommand(Guid.NewGuid(), null, preco));

        Assert.True(result.IsValid);
    }

    [Theory(DisplayName = "Atualizar: preço fora de numeric(10,2) é inválido")]
    [MemberData(nameof(PrecosForaDaColuna))]
    public void Atualizar_ShouldBeInvalid_WhenPrecoExceedsColumn(decimal preco)
    {
        var result = _atualizar.Validate(new AtualizarServicoCommand(Guid.NewGuid(), null, preco));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(AtualizarServicoCommand.Preco));
    }

    [Fact(DisplayName = "Atualizar: preço ausente não é validado")]
    public void Atualizar_ShouldBeValid_WhenPrecoIsNull()
    {
        var result = _atualizar.Validate(new AtualizarServicoCommand(Guid.NewGuid(), "nova descrição", null));

        Assert.True(result.IsValid);
    }
}
