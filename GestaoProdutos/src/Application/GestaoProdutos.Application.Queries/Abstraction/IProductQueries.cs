using GestaoProdutos.Application.Queries.Dtos;

namespace GestaoProdutos.Application.Queries.Abstraction;

public interface IProductQueries
{
    Task<ProductDetailsDto> GetProductByIdAsync(Guid id);
    Task<ProductDetailsDto> GetProductByCodeAsync(int code);
    Task<ProductListDetailsDto> GetProductsPagedAsync(ProductSearchableDto productSearchableDto);
}