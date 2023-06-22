using GestaoProdutos.Domain.Repositories.Dtos;

namespace GestaoProdutos.Domain.Repositories;

public interface IProductRepository
{
    Task CreateAsync(ProductCreateDto createDto);
    Task UpdateAsync(int productCode, ProductUpdateDto updateDto);
    Task DeleteAsync(int productCode);
}