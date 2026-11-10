using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Testcontainers.PostgreSql;

namespace IntegrationTests.Infrastructure;

/// <summary>
/// Factory compartilhada por todos os testes de integração via [Collection("Integration")].
/// Sobe um container PostgreSQL real (Testcontainers) uma única vez por sessão de testes,
/// aplica as migrations do <c>CadastroDbContext</c> automaticamente (via Program.cs, que já chama
/// MigrateAsync no startup) e fornece helpers de autenticação.
/// </summary>
public sealed class OficinaMecanicaWebApplicationFactory
    : WebApplicationFactory<Program>, IAsyncLifetime
{
    // Segredo de teste — o mesmo injetado na configuração do host e usado para assinar os tokens
    private const string TestJwtSecret = "integration-test-jwt-secret-minimum-32-chars!!";

    // Mesmos valores configurados via Jwt:Audience / Jwt:ValidIssuers abaixo — ver RFC-001 §4.1
    // (contrato do token) e ADR-003 (dois emissores). O serviço de autenticação e a Lambda
    // emitem os tokens em produção; aqui são montados diretamente.
    private const string AppIssuer = "oficina-mecanica-app";
    private const string ClienteAuthIssuer = "oficina-mecanica-auth";
    private const string Audience = "oficina-mecanica-api";

    private readonly PostgreSqlContainer _db = new PostgreSqlBuilder()
        .WithDatabase("cadastro")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    // IAsyncLifetime.InitializeAsync — xunit.v3 chama antes do primeiro teste da coleção
    public async ValueTask InitializeAsync()
    {
        await _db.StartAsync();
    }

    // Sobrescreve WebApplicationFactory.DisposeAsync e também satisfaz IAsyncLifetime.DisposeAsync
    // (ambos têm a mesma assinatura ValueTask DisposeAsync())
    public override async ValueTask DisposeAsync()
    {
        await _db.DisposeAsync();
        await base.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        // Sobrescreve configurações sensíveis com valores de teste.
        // ConfigureAppConfiguration adiciona fontes APÓS os defaults (appsettings.json),
        // então estes valores têm prioridade.
        builder.ConfigureAppConfiguration((_, cfg) =>
        {
            cfg.AddInMemoryCollection(new Dictionary<string, string?>
            {
                // Connection string aponta para o container Testcontainers
                ["ConnectionStrings:Default"] = _db.GetConnectionString(),
                // JWT secret — mesmo valor usado para gerar e validar tokens nos testes
                ["Jwt:Secret"] = TestJwtSecret,
                ["Jwt:Audience"] = Audience,
                ["Jwt:ValidIssuers:0"] = AppIssuer,
                ["Jwt:ValidIssuers:1"] = ClienteAuthIssuer,
                // A exposição do OpenAPI/Scalar não depende mais do ambiente (ver
                // Api.Extensions.OpenApiExtensions), então é desligada explicitamente aqui
                // para manter a suíte de integração rápida e sem rotas de documentação.
                ["OpenApi:Enabled"] = "false",
            });
        });
    }

    /// <summary>
    /// Cria um HttpClient com Authorization: Bearer {token} pré-configurado.
    /// </summary>
    public HttpClient CreateAuthenticatedClient(string token)
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    /// <summary>
    /// Gera um token de papel Oficina assinado com o segredo de teste, simulando o token que o
    /// serviço de autenticação (emissor "oficina-mecanica-app") emitiria — este serviço não emite
    /// tokens, só os valida.
    /// </summary>
    public string GetOficinaToken() =>
        GerarToken(AppIssuer, subject: "oficina-integration-test", role: "Oficina");

    /// <summary>
    /// Gera um token de papel Cliente assinado com o mesmo segredo de teste, simulando o
    /// token que a Function Serverless (emissor "oficina-mecanica-auth") emitiria — ver
    /// RFC-001 §4.1.
    /// </summary>
    public string GetClienteAuthToken(Guid clienteId) =>
        GerarToken(ClienteAuthIssuer, subject: clienteId.ToString(), role: "Cliente");

    /// <summary>
    /// Cria um HttpClient autenticado com um token de papel Cliente para o clienteId informado.
    /// </summary>
    public HttpClient CreateClienteAuthenticatedClient(Guid clienteId) =>
        CreateAuthenticatedClient(GetClienteAuthToken(clienteId));

    private static string GerarToken(string issuer, string subject, string role)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, subject),
            new Claim("role", role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestJwtSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

/// <summary>
/// Define a coleção "Integration" — todos os testes com [Collection("Integration")]
/// compartilham a mesma instância de OficinaMecanicaWebApplicationFactory.
/// xunit.v3 garante que os testes da coleção NÃO rodam em paralelo entre si.
/// </summary>
[CollectionDefinition("Integration")]
public class IntegrationTestCollection
    : ICollectionFixture<OficinaMecanicaWebApplicationFactory> { }
