using System.Runtime.CompilerServices;
using GestaoProdutos.Application.Queries.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using GestaoProdutos.Application.Queries.Dtos;
using GestaoProdutos.Domain.Entities;

[assembly:InternalsVisibleTo("GestaoProdutos.Application.Queries.Tests")]
namespace GestaoProdutos.Application.Queries.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueries(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddScoped<IProductQueries, ProductQueries>()
            .AddQueriesMapper();

        return serviceCollection;
    }

    public static IServiceCollection AddQueriesMapper(this IServiceCollection serviceCollection)
    {
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Product, ProductDetailsDto>()
                .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => src.Fornecedor));

            cfg.CreateMap<Provider, ProviderDetailsDto>();
        });

        var mapper = mapperConfiguration.CreateMapper();
        serviceCollection.AddSingleton(mapper);

        return serviceCollection;
    }
}