using GestaoProdutos.Application.Queries.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoProdutos.Application.Queries.Tests.Fixtures;

public class MapperFixture
{
    private readonly IServiceCollection _serviceCollection;
    
    public MapperFixture()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddQueriesMapper();

        _serviceCollection = serviceCollection;
    }

    public IServiceProvider BuildServiceProvider()
    {
        return _serviceCollection.BuildServiceProvider();
    }
}