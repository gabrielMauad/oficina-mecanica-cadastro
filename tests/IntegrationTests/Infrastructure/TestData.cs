using Cadastro.Domain.Veiculo;

namespace IntegrationTests.Infrastructure;

/// <summary>
/// Gerador de fixtures únicas para os testes de integração.
///
/// A coleção "Integration" compartilha um único PostgreSQL por sessão e nunca limpa os dados
/// entre testes. Com fixtures fixas no código, dois testes que usassem o mesmo CPF ou a mesma
/// placa quebravam um ao outro pelos índices únicos de <c>cadastro.cliente.documento</c> e
/// <c>cadastro.veiculo.placa</c>. Cada chamada aqui devolve um valor inédito e válido, então
/// nenhum teste depende do que outro deixou no banco.
///
/// Os documentos e placas gerados são conferidos contra a validação real do domínio
/// (Cadastro.Domain.Cliente.Cpf, Cadastro.Domain.Cliente.Cnpj e <see cref="Placa"/>) antes de serem devolvidos —
/// o gerador não reimplementa a regra, ele calcula o candidato e valida no próprio value object.
/// </summary>
public static class TestData
{
    private const int MaxTentativas = 100;

    private static readonly char[] Letras = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    /// <summary>
    /// Sufixo curto derivado de <see cref="Guid"/>, usado para tornar nomes, descrições
    /// e e-mails únicos sem deixá-los ilegíveis no relatório de falha.
    /// </summary>
    public static string Sufixo() => Guid.NewGuid().ToString("N")[..8];

    /// <summary>
    /// Sufixo curto derivado de <see cref="Guid"/> contendo apenas letras. Nome de cliente
    /// aceita somente letras e espaços (<c>^[\p{L}\s]+$</c> em CadastrarClienteValidator),
    /// então o sufixo hexadecimal não serve para nomes.
    /// </summary>
    public static string SufixoLetras()
    {
        var bytes = Guid.NewGuid().ToByteArray();
        return new string(bytes.Take(8).Select(b => (char)('a' + b % 26)).ToArray());
    }

    /// <summary>
    /// CPF válido: 9 dígitos sorteados + os 2 dígitos verificadores calculados.
    /// Sequências de dígitos repetidos (111..., 222...) são descartadas porque o validador
    /// do domínio as recusa.
    /// </summary>
    public static string Cpf()
    {
        for (var tentativa = 0; tentativa < MaxTentativas; tentativa++)
        {
            var digitos = new int[11];
            for (var i = 0; i < 9; i++)
                digitos[i] = Random.Shared.Next(10);

            if (TodosIguais(digitos, 9))
                continue;

            digitos[9] = DigitoVerificador(digitos, 9, pesoInicial: 10);
            digitos[10] = DigitoVerificador(digitos, 10, pesoInicial: 11);

            var candidato = string.Concat(digitos);

            // Confirma contra a validação real do domínio antes de devolver
            if (Cadastro.Domain.Cliente.Cpf.Criar(candidato).IsSuccess)
                return candidato;
        }

        throw new InvalidOperationException("Não foi possível gerar um CPF válido para o teste.");
    }

    /// <summary>
    /// CNPJ válido: 12 dígitos sorteados + os 2 dígitos verificadores calculados,
    /// pelo mesmo critério do CPF (módulo 11 com pesos cíclicos).
    /// </summary>
    public static string Cnpj()
    {
        int[] pesosPrimeiro = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] pesosSegundo = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

        for (var tentativa = 0; tentativa < MaxTentativas; tentativa++)
        {
            var digitos = new int[14];
            for (var i = 0; i < 12; i++)
                digitos[i] = Random.Shared.Next(10);

            if (TodosIguais(digitos, 12))
                continue;

            digitos[12] = DigitoVerificadorComPesos(digitos, pesosPrimeiro);
            digitos[13] = DigitoVerificadorComPesos(digitos, pesosSegundo);

            var candidato = string.Concat(digitos);

            // Confirma contra a validação real do domínio antes de devolver
            if (Cadastro.Domain.Cliente.Cnpj.Criar(candidato).IsSuccess)
                return candidato;
        }

        throw new InvalidOperationException("Não foi possível gerar um CNPJ válido para o teste.");
    }

    /// <summary>
    /// Placa única no formato Mercosul (ABC1D23), aceito por <see cref="Placa"/>.
    /// </summary>
    public static string PlacaMercosul() =>
        GerarPlaca(() => $"{Letra()}{Letra()}{Letra()}{Digito()}{Letra()}{Digito()}{Digito()}");

    /// <summary>
    /// Placa única no formato antigo com hífen (ABC-1234). O domínio normaliza removendo
    /// o hífen, então a placa persistida fica ABC1234.
    /// </summary>
    public static string PlacaAntiga() =>
        GerarPlaca(() => $"{Letra()}{Letra()}{Letra()}-{Digito()}{Digito()}{Digito()}{Digito()}");

    /// <summary>
    /// Nome único: o rótulo informado seguido de um sufixo só de letras
    /// (ex.: "João Silva kqzmbvxa") — formato aceito também por nome de cliente.
    /// </summary>
    public static string Nome(string rotulo) => $"{rotulo} {SufixoLetras()}";

    /// <summary>
    /// Descrição única: o texto informado seguido de um sufixo curto.
    /// </summary>
    public static string Descricao(string texto) => $"{texto} {Sufixo()}";

    /// <summary>
    /// E-mail único no domínio de testes (ex.: "joao.silva.a1b2c3d4@integration.test").
    /// </summary>
    public static string Email(string prefixo) => $"{prefixo}.{Sufixo()}@integration.test";

    // ── Internos ──

    private static string GerarPlaca(Func<string> montar)
    {
        for (var tentativa = 0; tentativa < MaxTentativas; tentativa++)
        {
            var candidato = montar();

            // Confirma contra a validação real do domínio antes de devolver
            if (Placa.Criar(candidato).IsSuccess)
                return candidato;
        }

        throw new InvalidOperationException("Não foi possível gerar uma placa válida para o teste.");
    }

    private static char Letra() => Letras[Random.Shared.Next(Letras.Length)];

    private static char Digito() => (char)('0' + Random.Shared.Next(10));

    private static bool TodosIguais(int[] digitos, int quantidade)
    {
        for (var i = 1; i < quantidade; i++)
            if (digitos[i] != digitos[0])
                return false;

        return true;
    }

    /// <summary>
    /// Dígito verificador de CPF: soma os <paramref name="quantidade"/> primeiros dígitos com
    /// pesos decrescentes a partir de <paramref name="pesoInicial"/> e aplica o módulo 11.
    /// </summary>
    private static int DigitoVerificador(int[] digitos, int quantidade, int pesoInicial)
    {
        var soma = 0;
        for (var i = 0; i < quantidade; i++)
            soma += digitos[i] * (pesoInicial - i);

        var resto = soma % 11;
        return resto < 2 ? 0 : 11 - resto;
    }

    /// <summary>
    /// Dígito verificador de CNPJ: soma os dígitos com a sequência de pesos informada
    /// e aplica o módulo 11.
    /// </summary>
    private static int DigitoVerificadorComPesos(int[] digitos, int[] pesos)
    {
        var soma = 0;
        for (var i = 0; i < pesos.Length; i++)
            soma += digitos[i] * pesos[i];

        var resto = soma % 11;
        return resto < 2 ? 0 : 11 - resto;
    }
}
