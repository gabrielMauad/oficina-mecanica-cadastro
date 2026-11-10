using SharedKernel.Domain;
using ClienteEntity = Cadastro.Domain.Cliente.Cliente;

namespace Cadastro.Domain.Tests.Cliente;

public class ClienteTests
{
    [Theory]
    [InlineData("Cliente 1", "632.582.650-70", "cliente1@example.com", "31999999999", true)]
    [InlineData("Cliente 2", "12.205.621/0001-90", "cliente2@example.com", "(31)98888-8888", false)]
    [InlineData("Cliente 3", "01404238000", "cliente3@example.com", "31977777777", true)]
    [InlineData("Cliente 4", "40843048000186", "cliente4@example.com.br", "31966666666", false)]
    public void Criar_ComDadosValidos_RetornaCliente(string nome, string documento, string email, string telefone, bool pessoaFisica)
    {
        // Act
        var clienteResult = ClienteEntity.Criar(nome, documento, email, telefone, pessoaFisica);

        // Assert
        Assert.True(clienteResult.IsSuccess);
        Assert.False(clienteResult.IsFailure);
        Assert.Equal(Error.None, clienteResult.Error);
        Assert.Equal(nome, clienteResult.Value.Nome);
        Assert.Equal(new string(documento.Where(char.IsDigit).ToArray()), clienteResult.Value.Documento.Numero);
        Assert.Equal(email, clienteResult.Value.Email);
        Assert.Equal(new string(telefone.Where(char.IsDigit).ToArray()), clienteResult.Value.Telefone);
        Assert.True(clienteResult.Value.Ativo);
        Assert.InRange(clienteResult.Value.CadastradoEm, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
        Assert.InRange(clienteResult.Value.AtualizadoEm, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);
        Assert.NotEqual(Guid.Empty, clienteResult.Value.Id.Value);
    }


    [Theory]
    [InlineData("", "632.582.650-70", "cliente1@example.com", "31999999999", true, "Cliente.NomeVazio", "Nome é obrigatório.")]
    [InlineData("   ", "632.582.650-70", "cliente1@example.com", "31999999999", true, "Cliente.NomeVazio", "Nome é obrigatório.")]
    [InlineData("Cliente 1", "632.582.650-70", "", "31999999999", true, "Cliente.EmailInvalido", "Email inválido.")]
    [InlineData("Cliente 1", "632.582.650-70", "    ", "31999999999", true, "Cliente.EmailInvalido", "Email inválido.")]
    [InlineData("Cliente 1", "632.582.650-70", ".com@email", "31999999999", true, "Cliente.EmailInvalido", "Email inválido.")]
    [InlineData("Cliente 1", "632.582.650-70", "cliente1@example.com", "", true, "Cliente.TelefoneInvalido", "Telefone inválido.")]
    [InlineData("Cliente 1", "632.582.650-70", "cliente1@example.com", "    ", true, "Cliente.TelefoneInvalido", "Telefone inválido.")]
    [InlineData("Cliente 1", "632.582.650-70", "cliente1@example.com", "12312312313123123", true, "Cliente.TelefoneInvalido", "Telefone inválido.")]
    [InlineData("Cliente 1", "632.582.650-70", "cliente1@example.com", "select * from dbo as 31999999999", true, "Cliente.TelefoneInvalido", "Telefone inválido.")]
    [InlineData("Cliente 1", "632.582.650-70", "cliente1@example.com", "3188888888", true, "Cliente.TelefoneInvalido", "Telefone inválido.")]
    [InlineData("Cliente 1", "632.582.650-70", "cliente1@example.com", "(31)8888-8888", true, "Cliente.TelefoneInvalido", "Telefone inválido.")]
    [InlineData("Cliente 1", "", "cliente1@example.com.br", "31999999999", true, "CPF.Invalido", "CPF é obrigatório.")]
    [InlineData("Cliente 1", "  ", "cliente1@example.com.br", "31999999999", true, "CPF.Invalido", "CPF é obrigatório.")]
    [InlineData("Cliente 1", "123.456.789-10", "cliente1@example.com.br", "31999999999", true, "CPF.Invalido", "CPF inválido.")]
    [InlineData("Cliente 1", "", "cliente1@example.com.br", "31999999999", false, "CNPJ.Invalido", "CNPJ é obrigatório.")]
    [InlineData("Cliente 1", "  ", "cliente1@example.com.br", "31999999999", false, "CNPJ.Invalido", "CNPJ é obrigatório.")]
    [InlineData("Cliente 1", "12.345.678/9101-11", "cliente1@example.com.br", "31999999999", false, "CNPJ.Invalido", "CNPJ inválido.")]
    public void Criar_ComDadosInvalidos_RetornaErro(
        string nome,
        string documento,
        string email,
        string telefone,
        bool pessoaFisica,
        string errorCode,
        string errorMessage
    )
    {
        // Act
        var clienteResult = ClienteEntity.Criar(nome, documento, email, telefone, pessoaFisica);

        // Assert
        Assert.False(clienteResult.IsSuccess);
        Assert.True(clienteResult.IsFailure);
        Assert.Equal(errorCode, clienteResult.Error.Code);
        Assert.Equal(errorMessage, clienteResult.Error.Message);
    }

    [Fact]
    public void AtualizarNome_ComNovoNome_AtualizaNome()
    {
        // Arrange
        var cliente = ClienteEntity.Criar("Cliente 1", "632.582.650-70", "cliente1@example.com", "31999999999", true).Value;

        // Act
        var clienteResult = cliente.AtualizarNome("Cliente Atualizado");

        // Assert
        Assert.True(clienteResult.IsSuccess);
        Assert.Equal("Cliente Atualizado", cliente.Nome);
    }

    [Theory]
    [InlineData("31988888888")]
    [InlineData("(31)98888-8888")]
    public void AtualizarTelefone_ComTelefoneValido_AtualizaTelefone(string novoTelefone)
    {
        // Arrange
        var cliente = ClienteEntity.Criar("Cliente 1", "632.582.650-70", "cliente1@example.com", "31999999999", true).Value;
        var telefoneEsperado = new string(novoTelefone.Where(char.IsDigit).ToArray());

        // Act
        var clienteResult = cliente.AtualizarTelefone(novoTelefone);

        // Assert
        Assert.True(clienteResult.IsSuccess);
        Assert.Equal(telefoneEsperado, cliente.Telefone);
    }

    [Fact]
    public void Desativar_ClienteAtivo_SetaAtivoComoFalso()
    {
        // Arrange
        var cliente = ClienteEntity.Criar("Cliente 1", "632.582.650-70", "cliente1@example.com", "31999999999", true).Value;

        // Act
        cliente.Desativar();

        // Assert
        Assert.False(cliente.Ativo);
    }
}

