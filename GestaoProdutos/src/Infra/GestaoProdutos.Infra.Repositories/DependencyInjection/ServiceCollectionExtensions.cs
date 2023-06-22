using GestaoProdutos.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoProdutos.Infra.Repositories.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddScoped<IProductRepository, ProductRepository>();

        return serviceCollection;
    }
}