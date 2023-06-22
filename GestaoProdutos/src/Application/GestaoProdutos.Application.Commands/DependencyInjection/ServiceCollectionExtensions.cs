using GestaoProdutos.Application.Commands.Abstraction;
using GestaoProdutos.Application.Commands.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoProdutos.Application.Commands.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandFactories(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddScoped<ICommandFactory, CommandFactory>();

        return serviceCollection;
    }
}