using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using IntegrationTests.Infrastructure;

namespace IntegrationTests.Modules.Cadastro;

[Collection("Integration")]
public class CadastroEndpointsTests
{
    private readonly OficinaMecanicaWebApplicationFactory _factory;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public CadastroEndpointsTests(OficinaMecanicaWebApplicationFactory factory)
    {
        _factory = factory;
    }

    // =========================================================================
    // POST /api/v1/clientes
    // =========================================================================

    [Fact(DisplayName = "POST /clientes — retorna 201 com dados do cliente criado")]
    public async Task CriarCliente_DeveRetornar201_QuandoDadosValidos()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        var nome = TestData.Nome("João Silva");
        var documento = TestData.Cpf();
        var email = TestData.Email("joao.silva");

        var command = new
        {
            Nome = nome,
            Documento = documento,
            Email = email,
            Telefone = "31999990001",
            PessoaFisica = true
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/clientes", command);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<ClienteCriadoResponse>(JsonOptions);
        Assert.NotNull(body);
        Assert.NotEqual(Guid.Empty, body.ClienteId);
        Assert.Equal(nome, body.Nome);
        Assert.Equal(documento, body.Documento);
        Assert.Equal(email, body.Email);
        Assert.True(body.Ativo);
    }

    [Fact(DisplayName = "POST /clientes sem token — retorna 401")]
    public async Task CriarCliente_SemAutenticacao_DeveRetornar401()
    {
        // Arrange — client sem token
        using var client = _factory.CreateClient();

        var command = new
        {
            Nome = TestData.Nome("Anônimo"),
            Documento = TestData.Cpf(),
            Email = TestData.Email("anonimo"),
            Telefone = "31999990000",
            PessoaFisica = true
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/clientes", command);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    // =========================================================================
    // GET /api/v1/clientes
    // =========================================================================

    [Fact(DisplayName = "GET /clientes — retorna 200 com lista contendo o cliente criado")]
    public async Task ListarClientes_DeveRetornar200_ComListaDeItens()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        var nome = TestData.Nome("Maria Santos");
        var documento = TestData.Cpf();

        var command = new
        {
            Nome = nome,
            Documento = documento,
            Email = TestData.Email("maria.santos"),
            Telefone = "31999990002",
            PessoaFisica = true
        };

        var createResponse = await client.PostAsJsonAsync("/api/v1/clientes", command);
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<ClienteCriadoResponse>(JsonOptions);

        // Act
        var response = await client.GetAsync("/api/v1/clientes");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<List<ClienteListItemResponse>>(JsonOptions);
        Assert.NotNull(body);
        Assert.NotEmpty(body);

        var cliente = body.FirstOrDefault(c => c.Id == created!.ClienteId);
        Assert.NotNull(cliente);
        Assert.Equal(nome, cliente.Nome);
        Assert.Equal(documento, cliente.Documento);
    }

    // =========================================================================
    // GET /api/v1/clientes/{id}
    // =========================================================================

    [Fact(DisplayName = "GET /clientes/{id} — retorna 200 com dados do cliente")]
    public async Task ObterCliente_DeveRetornar200_QuandoClienteExiste()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        var nome = TestData.Nome("Pedro Costa");
        var documento = TestData.Cpf();
        var email = TestData.Email("pedro.costa");

        var command = new
        {
            Nome = nome,
            Documento = documento,
            Email = email,
            Telefone = "31999990003",
            PessoaFisica = true
        };

        var createResponse = await client.PostAsJsonAsync("/api/v1/clientes", command);
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<ClienteCriadoResponse>(JsonOptions);
        var clienteId = created!.ClienteId;

        // Act
        var response = await client.GetAsync($"/api/v1/clientes/{clienteId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<ClienteGetResponse>(JsonOptions);
        Assert.NotNull(body);
        Assert.Equal(clienteId, body.Id);
        Assert.Equal(nome, body.Nome);
        Assert.Equal(documento, body.Documento);
        Assert.Equal(email, body.Email);
        Assert.True(body.Ativo);
    }

    [Fact(DisplayName = "GET /clientes/{id} inexistente — retorna 404")]
    public async Task ObterCliente_DeveRetornar404_QuandoNaoExiste()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        // Act
        var response = await client.GetAsync($"/api/v1/clientes/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // =========================================================================
    // GET /api/v1/clientes/documento/{documento}
    // =========================================================================

    [Fact(DisplayName = "GET /clientes/documento/{documento} — retorna 200 com o mesmo ViewModel de GET /clientes/{id}")]
    public async Task ObterClientePorDocumento_DeveRetornar200_QuandoClienteExiste()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        var nome = TestData.Nome("Helena Documento");
        var documento = TestData.Cpf();
        var email = TestData.Email("helena.documento");

        var command = new
        {
            Nome = nome,
            Documento = documento,
            Email = email,
            Telefone = "31999990020",
            PessoaFisica = true
        };

        var createResponse = await client.PostAsJsonAsync("/api/v1/clientes", command);
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<ClienteCriadoResponse>(JsonOptions);
        var clienteId = created!.ClienteId;

        // Act
        var response = await client.GetAsync($"/api/v1/clientes/documento/{documento}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<ClienteGetResponse>(JsonOptions);
        Assert.NotNull(body);
        Assert.Equal(clienteId, body.Id);
        Assert.Equal(nome, body.Nome);
        Assert.Equal(documento, body.Documento);
        Assert.Equal(email, body.Email);
        Assert.True(body.Ativo);

        var porId = await client.GetFromJsonAsync<ClienteGetResponse>($"/api/v1/clientes/{clienteId}", JsonOptions);
        Assert.Equal(porId, body);
    }

    [Fact(DisplayName = "GET /clientes/documento/{documento} inexistente — retorna 404")]
    public async Task ObterClientePorDocumento_DeveRetornar404_QuandoNaoExiste()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        // Act
        var response = await client.GetAsync($"/api/v1/clientes/documento/{TestData.Cpf()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact(DisplayName = "GET /clientes/documento/{documento} com caracteres não numéricos — retorna 404")]
    public async Task ObterClientePorDocumento_DeveRetornar404_QuandoDocumentoNaoENumerico()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        // Act
        var response = await client.GetAsync("/api/v1/clientes/documento/123.456.789-09");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact(DisplayName = "GET /clientes/documento/{documento} com papel Cliente — retorna 403")]
    public async Task ObterClientePorDocumento_DeveRetornar403_QuandoPapelNaoEOficina()
    {
        // Arrange
        using var client = _factory.CreateClienteAuthenticatedClient(Guid.NewGuid());

        // Act
        var response = await client.GetAsync($"/api/v1/clientes/documento/{TestData.Cpf()}");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact(DisplayName = "GET /clientes/documento/{documento} sem token — retorna 401")]
    public async Task ObterClientePorDocumento_DeveRetornar401_SemAutenticacao()
    {
        // Arrange
        using var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/v1/clientes/documento/{TestData.Cpf()}");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    // =========================================================================
    // PATCH /api/v1/clientes/{id}/nome
    // =========================================================================

    [Fact(DisplayName = "PATCH /clientes/{id}/nome — atualiza nome e retorna 200")]
    public async Task AtualizarNomeCliente_DeveRetornar200_ComNovoNome()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        var command = new
        {
            Nome = TestData.Nome("Carlos Antigo"),
            Documento = TestData.Cpf(),
            Email = TestData.Email("carlos"),
            Telefone = "31999990004",
            PessoaFisica = true
        };

        var createResponse = await client.PostAsJsonAsync("/api/v1/clientes", command);
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<ClienteCriadoResponse>(JsonOptions);
        var clienteId = created!.ClienteId;

        // Act
        var patchContent = JsonContent.Create(new { Nome = "Carlos Novo" });
        var response = await client.PatchAsync($"/api/v1/clientes/{clienteId}/nome", patchContent);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<ClienteAtualizadoResponse>(JsonOptions);
        Assert.NotNull(body);
        Assert.Equal(clienteId, body.ClienteId);
        Assert.Equal("Carlos Novo", body.Nome);
    }

    // =========================================================================
    // PATCH /api/v1/clientes/{id}/telefone
    // =========================================================================

    [Fact(DisplayName = "PATCH /clientes/{id}/telefone — atualiza telefone e retorna 200")]
    public async Task AtualizarTelefoneCliente_DeveRetornar200_ComNovoTelefone()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        var command = new
        {
            Nome = TestData.Nome("Ana Lima"),
            Documento = TestData.Cpf(),
            Email = TestData.Email("ana.lima"),
            Telefone = "31999990005",
            PessoaFisica = true
        };

        var createResponse = await client.PostAsJsonAsync("/api/v1/clientes", command);
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<ClienteCriadoResponse>(JsonOptions);
        var clienteId = created!.ClienteId;

        // Act
        var patchContent = JsonContent.Create(new { Telefone = "31988880005" });
        var response = await client.PatchAsync($"/api/v1/clientes/{clienteId}/telefone", patchContent);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<ClienteAtualizadoResponse>(JsonOptions);
        Assert.NotNull(body);
        Assert.Equal(clienteId, body.ClienteId);
        Assert.Equal("31988880005", body.Telefone);
    }

    // =========================================================================
    // DELETE /api/v1/clientes/{id}
    // =========================================================================

    [Fact(DisplayName = "DELETE /clientes/{id} — desativa o cliente e retorna 204")]
    public async Task DesativarCliente_DeveRetornar204_EClienteFinicaInativo()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        var command = new
        {
            Nome = TestData.Nome("Lucas Temporário"),
            Documento = TestData.Cpf(),
            Email = TestData.Email("lucas"),
            Telefone = "31999990006",
            PessoaFisica = true
        };

        var createResponse = await client.PostAsJsonAsync("/api/v1/clientes", command);
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<ClienteCriadoResponse>(JsonOptions);
        var clienteId = created!.ClienteId;

        // Act
        var deleteResponse = await client.DeleteAsync($"/api/v1/clientes/{clienteId}");

        // Assert — 204 No Content
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // Soft delete: cliente ainda existe mas ativo = false
        var getResponse = await client.GetAsync($"/api/v1/clientes/{clienteId}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var body = await getResponse.Content.ReadFromJsonAsync<ClienteGetResponse>(JsonOptions);
        Assert.NotNull(body);
        Assert.False(body.Ativo);
    }

    [Fact(DisplayName = "DELETE /clientes/{id} inexistente — retorna 404")]
    public async Task DesativarCliente_DeveRetornar404_QuandoNaoExiste()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        // Act
        var response = await client.DeleteAsync($"/api/v1/clientes/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // =========================================================================
    // POST /api/v1/veiculos
    // =========================================================================

    [Fact(DisplayName = "POST /veiculos — retorna 201 com dados do veículo criado")]
    public async Task CriarVeiculo_DeveRetornar201_QuandoDadosValidos()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        var clienteCommand = new
        {
            Nome = TestData.Nome("Bruno Motorista"),
            Documento = TestData.Cpf(),
            Email = TestData.Email("bruno"),
            Telefone = "31999990007",
            PessoaFisica = true
        };

        // CPF gerado por TestData é inédito, então a criação sempre retorna 201
        var clienteResponse = await client.PostAsJsonAsync("/api/v1/clientes", clienteCommand);
        clienteResponse.EnsureSuccessStatusCode();
        var clienteCriado = await clienteResponse.Content.ReadFromJsonAsync<ClienteCriadoResponse>(JsonOptions);
        var clienteId = clienteCriado!.ClienteId;

        var placa = TestData.PlacaMercosul();

        var veiculoCommand = new
        {
            Placa = placa,
            Modelo = "Civic",
            Marca = "Honda",
            Ano = 2022,
            ClienteId = clienteId
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/veiculos", veiculoCommand);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<VeiculoCriadoResponse>(JsonOptions);
        Assert.NotNull(body);
        Assert.NotEqual(Guid.Empty, body.VeiculoId);
        Assert.Equal(placa, body.Placa);
        Assert.Equal("Civic", body.Modelo);
        Assert.Equal("Honda", body.Marca);
        Assert.Equal(2022, body.Ano);
        Assert.Equal(clienteId, body.ClienteId);
    }

    // =========================================================================
    // GET /api/v1/veiculos
    // =========================================================================

    [Fact(DisplayName = "GET /veiculos — retorna 200 com lista contendo o veículo criado")]
    public async Task ListarVeiculos_DeveRetornar200_ComListaDeItens()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        // Criar cliente dono do veículo
        var clienteCommand = new
        {
            Nome = TestData.Nome("Diego Viajante"),
            Documento = TestData.Cpf(),
            Email = TestData.Email("diego"),
            Telefone = "31999990008",
            PessoaFisica = true
        };

        var clienteResponse = await client.PostAsJsonAsync("/api/v1/clientes", clienteCommand);
        clienteResponse.EnsureSuccessStatusCode();
        var clienteCriado = await clienteResponse.Content.ReadFromJsonAsync<ClienteCriadoResponse>(JsonOptions);

        var placa = TestData.PlacaMercosul();

        var veiculoCommand = new
        {
            Placa = placa,
            Modelo = "Corolla",
            Marca = "Toyota",
            Ano = 2021,
            ClienteId = clienteCriado!.ClienteId
        };

        var createResponse = await client.PostAsJsonAsync("/api/v1/veiculos", veiculoCommand);
        createResponse.EnsureSuccessStatusCode();
        var veiculoCriado = await createResponse.Content.ReadFromJsonAsync<VeiculoCriadoResponse>(JsonOptions);

        // Act
        var response = await client.GetAsync("/api/v1/veiculos");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<List<VeiculoListItemResponse>>(JsonOptions);
        Assert.NotNull(body);
        Assert.NotEmpty(body);

        var veiculo = body.FirstOrDefault(v => v.Id == veiculoCriado!.VeiculoId);
        Assert.NotNull(veiculo);
        Assert.Equal(placa, veiculo.Placa);
        Assert.Equal("Corolla", veiculo.Modelo);
    }

    // =========================================================================
    // GET /api/v1/veiculos/{id}
    // =========================================================================

    [Fact(DisplayName = "GET /veiculos/{id} — retorna 200 com dados do veículo")]
    public async Task ObterVeiculo_DeveRetornar200_QuandoVeiculoExiste()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        var clienteCommand = new
        {
            Nome = TestData.Nome("Fernanda Condutora"),
            Documento = TestData.Cpf(),
            Email = TestData.Email("fernanda"),
            Telefone = "31999990009",
            PessoaFisica = true
        };

        var clienteResponse = await client.PostAsJsonAsync("/api/v1/clientes", clienteCommand);
        clienteResponse.EnsureSuccessStatusCode();
        var clienteCriado = await clienteResponse.Content.ReadFromJsonAsync<ClienteCriadoResponse>(JsonOptions);

        var placa = TestData.PlacaMercosul();

        var veiculoCommand = new
        {
            Placa = placa,
            Modelo = "Fit",
            Marca = "Honda",
            Ano = 2020,
            ClienteId = clienteCriado!.ClienteId
        };

        var createResponse = await client.PostAsJsonAsync("/api/v1/veiculos", veiculoCommand);
        createResponse.EnsureSuccessStatusCode();
        var veiculoCriado = await createResponse.Content.ReadFromJsonAsync<VeiculoCriadoResponse>(JsonOptions);
        var veiculoId = veiculoCriado!.VeiculoId;

        // Act
        var response = await client.GetAsync($"/api/v1/veiculos/{veiculoId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<VeiculoGetResponse>(JsonOptions);
        Assert.NotNull(body);
        Assert.Equal(veiculoId, body.Id);
        Assert.Equal(placa, body.Placa);
        Assert.Equal("Fit", body.Modelo);
        Assert.Equal("Honda", body.Marca);
        Assert.Equal(2020, body.Ano);
        Assert.Equal(clienteCriado.ClienteId, body.ClienteId);
    }

    [Fact(DisplayName = "GET /veiculos/{id} inexistente — retorna 404")]
    public async Task ObterVeiculo_DeveRetornar404_QuandoNaoExiste()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        // Act
        var response = await client.GetAsync($"/api/v1/veiculos/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // =========================================================================
    // GET /api/v1/clientes/{id}/veiculos
    // =========================================================================

    [Fact(DisplayName = "GET /clientes/{id}/veiculos — retorna 200 com veículos do cliente")]
    public async Task ListarVeiculosPorCliente_DeveRetornar200_ComVeiculosDoCliente()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        var nomeCliente = TestData.Nome("Gabriel Frota");

        var clienteCommand = new
        {
            Nome = nomeCliente,
            Documento = TestData.Cpf(),
            Email = TestData.Email("gabriel"),
            Telefone = "31999990010",
            PessoaFisica = true
        };

        var clienteResponse = await client.PostAsJsonAsync("/api/v1/clientes", clienteCommand);
        clienteResponse.EnsureSuccessStatusCode();
        var clienteCriado = await clienteResponse.Content.ReadFromJsonAsync<ClienteCriadoResponse>(JsonOptions);
        var clienteId = clienteCriado!.ClienteId;

        // Criar dois veículos para este cliente
        var placas = new[] { TestData.PlacaMercosul(), TestData.PlacaMercosul() };

        foreach (var placa in placas)
        {
            var veiculoCommand = new
            {
                Placa = placa,
                Modelo = "Onix",
                Marca = "Chevrolet",
                Ano = 2023,
                ClienteId = clienteId
            };
            var v = await client.PostAsJsonAsync("/api/v1/veiculos", veiculoCommand);
            v.EnsureSuccessStatusCode();
        }

        // Act
        var response = await client.GetAsync($"/api/v1/clientes/{clienteId}/veiculos");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<VeiculosPorClienteResponse>(JsonOptions);
        Assert.NotNull(body);
        Assert.Equal(clienteId, body.ClienteId);
        Assert.Equal(nomeCliente, body.NomeCliente);
        Assert.Equal(2, body.Veiculos.Count);
        Assert.Contains(body.Veiculos, v => v.Placa == placas[0]);
        Assert.Contains(body.Veiculos, v => v.Placa == placas[1]);
    }

    [Fact(DisplayName = "GET /clientes/{id}/veiculos — retorna 404 quando cliente não existe")]
    public async Task ListarVeiculosPorCliente_DeveRetornar404_QuandoClienteNaoExiste()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        // Act
        var response = await client.GetAsync($"/api/v1/clientes/{Guid.NewGuid()}/veiculos");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // =========================================================================
    // POST /api/v1/servicos
    // =========================================================================

    [Fact(DisplayName = "POST /servicos — retorna 201 com dados do serviço criado")]
    public async Task CriarServico_DeveRetornar201_QuandoDadosValidos()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        var nome = TestData.Nome("Troca de Óleo");
        var descricao = TestData.Descricao("Troca completa com filtro");

        var command = new
        {
            Nome = nome,
            Descricao = descricao,
            Preco = 150.00m
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/servicos", command);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<ServicoCriadoResponse>(JsonOptions);
        Assert.NotNull(body);
        Assert.NotEqual(Guid.Empty, body.ServicoId);
        Assert.Equal(nome, body.Nome);
        Assert.Equal(descricao, body.Descricao);
        Assert.Equal(150.00m, body.PrecoBase);
        Assert.True(body.Ativo);
    }

    // =========================================================================
    // GET /api/v1/servicos
    // =========================================================================

    [Fact(DisplayName = "GET /servicos — retorna 200 com lista contendo o serviço criado")]
    public async Task ListarServicos_DeveRetornar200_ComListaDeItens()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        var nome = TestData.Nome("Alinhamento e Balanceamento");

        var command = new
        {
            Nome = nome,
            Descricao = (string?)null,
            Preco = 120.00m
        };

        var createResponse = await client.PostAsJsonAsync("/api/v1/servicos", command);
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<ServicoCriadoResponse>(JsonOptions);

        // Act
        var response = await client.GetAsync("/api/v1/servicos");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<List<ServicoListItemResponse>>(JsonOptions);
        Assert.NotNull(body);
        Assert.NotEmpty(body);

        var servico = body.FirstOrDefault(s => s.Id == created!.ServicoId);
        Assert.NotNull(servico);
        Assert.Equal(nome, servico.Nome);
        Assert.Equal(120.00m, servico.Preco);
    }

    // =========================================================================
    // GET /api/v1/servicos/{id}
    // =========================================================================

    [Fact(DisplayName = "GET /servicos/{id} — retorna 200 com dados do serviço")]
    public async Task ObterServico_DeveRetornar200_QuandoServicoExiste()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        var nome = TestData.Nome("Revisão de Freios");
        var descricao = TestData.Descricao("Pastilhas e discos dianteiros");

        var command = new
        {
            Nome = nome,
            Descricao = descricao,
            Preco = 350.00m
        };

        var createResponse = await client.PostAsJsonAsync("/api/v1/servicos", command);
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<ServicoCriadoResponse>(JsonOptions);
        var servicoId = created!.ServicoId;

        // Act
        var response = await client.GetAsync($"/api/v1/servicos/{servicoId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<ServicoGetResponse>(JsonOptions);
        Assert.NotNull(body);
        Assert.Equal(servicoId, body.Id);
        Assert.Equal(nome, body.Nome);
        Assert.Equal(descricao, body.Descricao);
        Assert.Equal(350.00m, body.Preco);
        Assert.True(body.Ativo);
    }

    [Fact(DisplayName = "GET /servicos/{id} inexistente — retorna 404")]
    public async Task ObterServico_DeveRetornar404_QuandoNaoExiste()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        // Act
        var response = await client.GetAsync($"/api/v1/servicos/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // =========================================================================
    // PATCH /api/v1/servicos/{id}/descricao
    // =========================================================================

    [Fact(DisplayName = "PATCH /servicos/{id}/descricao — atualiza descrição e retorna 200")]
    public async Task AtualizarDescricaoServico_DeveRetornar200_ComNovaDescricao()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        var command = new
        {
            Nome = TestData.Nome("Diagnóstico Eletrônico"),
            Descricao = TestData.Descricao("Descrição original"),
            Preco = 200.00m
        };

        var createResponse = await client.PostAsJsonAsync("/api/v1/servicos", command);
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<ServicoCriadoResponse>(JsonOptions);
        var servicoId = created!.ServicoId;

        // Act
        var patchContent = JsonContent.Create(new { Descricao = "Leitura de falhas com scanner" });
        var response = await client.PatchAsync($"/api/v1/servicos/{servicoId}/descricao", patchContent);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<ServicoAtualizadoResponse>(JsonOptions);
        Assert.NotNull(body);
        Assert.Equal(servicoId, body.ServicoId);
        Assert.Equal("Leitura de falhas com scanner", body.Descricao);
    }

    // =========================================================================
    // PATCH /api/v1/servicos/{id}/preco
    // =========================================================================

    [Fact(DisplayName = "PATCH /servicos/{id}/preco — atualiza preço e retorna 200")]
    public async Task AtualizarPrecoServico_DeveRetornar200_ComNovoPreco()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        var command = new
        {
            Nome = TestData.Nome("Lavagem Completa"),
            Descricao = (string?)null,
            Preco = 80.00m
        };

        var createResponse = await client.PostAsJsonAsync("/api/v1/servicos", command);
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<ServicoCriadoResponse>(JsonOptions);
        var servicoId = created!.ServicoId;

        // Act
        var patchContent = JsonContent.Create(new { Preco = 95.00m });
        var response = await client.PatchAsync($"/api/v1/servicos/{servicoId}/preco", patchContent);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<ServicoAtualizadoResponse>(JsonOptions);
        Assert.NotNull(body);
        Assert.Equal(servicoId, body.ServicoId);
        Assert.Equal(95.00m, body.Preco);
    }

    // =========================================================================
    // DELETE /api/v1/servicos/{id}
    // =========================================================================

    [Fact(DisplayName = "DELETE /servicos/{id} — desativa o serviço e retorna 204")]
    public async Task DesativarServico_DeveRetornar204_EServicoFicaInativo()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        var command = new
        {
            Nome = TestData.Nome("Polimento Temporário"),
            Descricao = (string?)null,
            Preco = 300.00m
        };

        var createResponse = await client.PostAsJsonAsync("/api/v1/servicos", command);
        createResponse.EnsureSuccessStatusCode();
        var created = await createResponse.Content.ReadFromJsonAsync<ServicoCriadoResponse>(JsonOptions);
        var servicoId = created!.ServicoId;

        // Act
        var deleteResponse = await client.DeleteAsync($"/api/v1/servicos/{servicoId}");

        // Assert — 204 No Content
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // Soft delete: serviço ainda existe mas ativo = false
        var getResponse = await client.GetAsync($"/api/v1/servicos/{servicoId}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var body = await getResponse.Content.ReadFromJsonAsync<ServicoGetResponse>(JsonOptions);
        Assert.NotNull(body);
        Assert.False(body.Ativo);
    }

    [Fact(DisplayName = "DELETE /servicos/{id} inexistente — retorna 404")]
    public async Task DesativarServico_DeveRetornar404_QuandoNaoExiste()
    {
        // Arrange
        var token = _factory.GetOficinaToken();
        using var client = _factory.CreateAuthenticatedClient(token);

        // Act
        var response = await client.DeleteAsync($"/api/v1/servicos/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // =========================================================================
    // DTOs locais para desserialização
    // Padrão do projeto: POSTs retornam *Id, GETs retornam Id
    // =========================================================================

    // --- Clientes ---
    private sealed record ClienteCriadoResponse(
        Guid ClienteId,
        string Nome,
        string Documento,
        string Email,
        string Telefone,
        bool Ativo,
        DateTime CadastradoEm,
        DateTime AtualizadoEm);

    private sealed record ClienteGetResponse(
        Guid Id,
        string Nome,
        string Documento,
        string Email,
        string Telefone,
        bool Ativo,
        DateTime CadastradoEm,
        DateTime AtualizadoEm);

    private sealed record ClienteListItemResponse(
        Guid Id,
        string Nome,
        string Documento,
        string Email,
        string Telefone,
        bool Ativo,
        DateTime CadastradoEm,
        DateTime AtualizadoEm);

    // PATCH /nome e /telefone → "clienteId"
    private sealed record ClienteAtualizadoResponse(
        Guid ClienteId,
        string Nome,
        string Telefone,
        bool Ativo,
        DateTime CadastradoEm,
        DateTime AtualizadoEm);

    // --- Veículos ---
    private sealed record VeiculoCriadoResponse(
        Guid VeiculoId,
        string Placa,
        string Modelo,
        string Marca,
        int Ano,
        Guid ClienteId,
        DateTime CadastradoEm,
        DateTime AtualizadoEm);

    private sealed record VeiculoGetResponse(
        Guid Id,
        string Placa,
        string Modelo,
        string Marca,
        int Ano,
        Guid ClienteId,
        DateTime CadastradoEm,
        DateTime AtualizadoEm);

    private sealed record VeiculoListItemResponse(
        Guid Id,
        string Placa,
        string Modelo,
        string Marca,
        int Ano,
        Guid ClienteId,
        DateTime CadastradoEm,
        DateTime AtualizadoEm);

    private sealed record VeiculosPorClienteResponse(
        Guid ClienteId,
        string NomeCliente,
        IReadOnlyList<VeiculoDoClienteResponse> Veiculos);

    private sealed record VeiculoDoClienteResponse(
        Guid VeiculoId,
        string Placa,
        string Modelo,
        string Marca,
        int Ano,
        DateTime CadastradoEm,
        DateTime AtualizadoEm);

    // --- Serviços ---
    private sealed record ServicoCriadoResponse(
        Guid ServicoId,
        string Nome,
        string? Descricao,
        decimal PrecoBase,
        bool Ativo,
        DateTime CadastradoEm,
        DateTime AtualizadoEm);

    private sealed record ServicoGetResponse(
        Guid Id,
        string Nome,
        string? Descricao,
        decimal Preco,
        bool Ativo,
        DateTime CadastradoEm,
        DateTime AtualizadoEm);

    private sealed record ServicoListItemResponse(
        Guid Id,
        string Nome,
        string? Descricao,
        decimal Preco,
        bool Ativo,
        DateTime CadastradoEm,
        DateTime AtualizadoEm);

    // PATCH /descricao e /preco → "servicoId"
    private sealed record ServicoAtualizadoResponse(
        Guid ServicoId,
        string? Descricao,
        decimal Preco,
        bool Ativo,
        DateTime CadastradoEm,
        DateTime AtualizadoEm);
}
