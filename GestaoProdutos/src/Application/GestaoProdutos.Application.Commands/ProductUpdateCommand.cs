using GestaoProdutos.Application.Commands.Abstraction;
using GestaoProdutos.Domain.Repositories;
using GestaoProdutos.Domain.Repositories.Dtos;

namespace GestaoProdutos.Application.Commands;

internal sealed class ProductUpdateCommand : ICommand
{
    private readonly IProductRepository _repository;
    private readonly int _productCode;
    private readonly ProductUpdateDto _updateDto;
    
    public ProductUpdateCommand(
        IProductRepository repository,
        int productCode,
        ProductUpdateDto updateDto)
    {
        _repository = repository;
        _productCode = productCode;
        _updateDto = updateDto;
    }

    public async Task ExecuteAsync() => await _repository.UpdateAsync(_productCode, _updateDto);
}