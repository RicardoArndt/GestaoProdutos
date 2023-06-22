using GestaoProdutos.Application.Commands.Abstraction;
using GestaoProdutos.Domain.Repositories;

namespace GestaoProdutos.Application.Commands;

internal sealed class ProductDeleteCommand : ICommand
{
    private readonly IProductRepository _repository;
    private readonly int _productCode;
    
    public ProductDeleteCommand(
        IProductRepository repository,
        int productCode)
    {
        _repository = repository;
        _productCode = productCode;
    }

    public async Task ExecuteAsync() => await _repository.DeleteAsync(_productCode);
}