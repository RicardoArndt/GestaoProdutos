using GestaoProdutos.Infra.Database.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoProdutos.Infra.Database.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSqlContext(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IContext, SqlContext>();

        return serviceCollection;
    }
}