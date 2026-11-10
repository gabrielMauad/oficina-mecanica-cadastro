using Cadastro.Adapters.Controllers;
using Cadastro.Adapters.DataSources;
using Cadastro.Adapters.Gateways;
using Cadastro.Application.Clientes.Commands.CadastrarCliente;
using Cadastro.Application.Clientes.Queries.ListarClientes;
using Cadastro.Application.Gateways;
using Cadastro.Application.Servicos.Queries.ListarServicos;
using Cadastro.Application.Veiculos.Queries.ListarVeiculos;
using Cadastro.Application.Veiculos.Queries.ListarVeiculosPorCliente;
using Cadastro.Contracts.Queries;
using Cadastro.Infrastructure.Persistence;
using Cadastro.Infrastructure.Persistence.Repositories;
using Cadastro.Infrastructure.Queries;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application;

namespace Cadastro.Infrastructure;

public static class CadastroModule
{
    public static IServiceCollection AddCadastroModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CadastroDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Default")));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<CadastroDbContext>());

        #region DataSources (Repositórios EF)

        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IVeiculoRepository, VeiculoRepository>();
        services.AddScoped<IServicoRepository, ServicoRepository>();

        #endregion

        #region Gateways (Interface Adapters)

        services.AddScoped<IClienteGateway, ClienteGateway>();
        services.AddScoped<IVeiculoGateway, VeiculoGateway>();
        services.AddScoped<IServicoGateway, ServicoGateway>();

        #endregion

        #region Controllers CA

        services.AddScoped<ClienteController>();
        services.AddScoped<ServicoController>();
        services.AddScoped<VeiculoController>();

        #endregion

        #region Queries de leitura

        services.AddScoped<IListarClientesQuery, ListarClientesQueryImpl>();
        services.AddScoped<IListarServicosQuery, ListarServicosQueryImpl>();
        services.AddScoped<IListarVeiculosQuery, ListarVeiculosQueryImpl>();
        services.AddScoped<IListarVeiculosPorClienteQuery, ListarVeiculosPorClienteQueryImpl>();

        #endregion

        #region Queries públicas (contratos expostos aos outros módulos)

        services.AddScoped<ICadastroClienteQuery, CadastroClienteQuery>();
        services.AddScoped<ICadastroServicoQuery, CadastroServicoQuery>();
        services.AddScoped<ICadastroVeiculoQuery, CadastroVeiculoQuery>();

        #endregion

        var applicationAssembly = typeof(CadastrarClienteCommand).Assembly;

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(applicationAssembly);
            cfg.AddOpenBehavior(typeof(SharedKernel.Application.Behaviors.LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(SharedKernel.Application.Behaviors.ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(SharedKernel.Application.Behaviors.TransactionBehavior<,>));
        });

        services.AddValidatorsFromAssembly(applicationAssembly);

        return services;
    }
}
