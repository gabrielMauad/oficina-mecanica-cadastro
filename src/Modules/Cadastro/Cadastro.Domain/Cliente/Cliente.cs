using System.Text.RegularExpressions;
using SharedKernel.Domain;

namespace Cadastro.Domain.Cliente;
public sealed class Cliente : AggregateRoot<ClienteId> 
{
    public string Nome { get; private set; }
    public Documento Documento { get; private set; }
    public string Email { get; private set; }
    public string Telefone { get; private set; }
    public bool Ativo { get; private set; }
    public DateTime CadastradoEm { get; private set; }
    public DateTime AtualizadoEm { get; private set; }

    private Cliente(ClienteId id, string nome, Documento documento, string email, string telefone)
       : base(id)
    {
        Nome = nome;
        Documento = documento;
        Email = email;
        Telefone = telefone;
        Ativo = true;
        CadastradoEm = DateTime.UtcNow;
        AtualizadoEm = DateTime.UtcNow;
    }

    private Cliente(ClienteId id, string nome, Documento documento, string email, string telefone,
        bool ativo, DateTime cadastradoEm, DateTime atualizadoEm) : base(id)
    {
        Nome = nome;
        Documento = documento;
        Email = email;
        Telefone = telefone;
        Ativo = ativo;
        CadastradoEm = cadastradoEm;
        AtualizadoEm = atualizadoEm;
    }

    public static Cliente Reconstituir(ClienteId id, string nome, Documento documento, string email,
        string telefone, bool ativo, DateTime cadastradoEm, DateTime atualizadoEm) =>
        new(id, nome, documento, email, telefone, ativo, cadastradoEm, atualizadoEm);

    public static Result<Cliente> Criar(string nome, string documento, string email, string telefone, bool pessoaFisica)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Error.Validation("Cliente.NomeVazio", "Nome é obrigatório.");
        if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            return Error.Validation("Cliente.EmailInvalido", "Email inválido.");
        if (string.IsNullOrWhiteSpace(telefone) || !Regex.IsMatch(telefone, @"^[\d\s()\-]+$"))
            return Error.Validation("Cliente.TelefoneInvalido", "Telefone inválido.");

        var telefoneNormalizado = new string(telefone.Where(char.IsDigit).ToArray());

        if (!Regex.IsMatch(telefoneNormalizado, @"^\d{2}9\d{8}$"))
            return Error.Validation("Cliente.TelefoneInvalido", "Telefone inválido.");

        Documento doc;
        if (pessoaFisica)
        {
            var cpfResult = Cpf.Criar(documento);
            if (cpfResult.IsFailure) return cpfResult.Error;
            doc = cpfResult.Value;
        }
        else
        {
            var cnpjResult = Cnpj.Criar(documento);
            if (cnpjResult.IsFailure) return cnpjResult.Error;
            doc = cnpjResult.Value;
        }

        var cliente = new Cliente(ClienteId.Novo(), nome, doc, email, telefoneNormalizado);

        return cliente;
    }

    public Result<Cliente> AtualizarTelefone(string novoTelefone)
    {
        Telefone = new string(novoTelefone.Where(char.IsDigit).ToArray());
        AtualizadoEm = DateTime.UtcNow;
        return this;
    }

    public Result<Cliente> AtualizarNome(string novoNome)
    {
        Nome = novoNome;
        AtualizadoEm = DateTime.UtcNow;
        return this;
    }

    public void Desativar()
    {
        Ativo = false;
        AtualizadoEm = DateTime.UtcNow;
    }
}