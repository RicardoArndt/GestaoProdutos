using GestaoProdutos.Application.Commands.Abstraction;
using GestaoProdutos.Domain.Repositories;
using GestaoProdutos.Domain.Repositories.Dtos;

namespace GestaoProdutos.Application.Commands;

internal sealed class ProductCreateCommand : ICommand
{
    private readonly IProductRepository _repository;
    private readonly ProductCreateDto _createDto;
    
    public ProductCreateCommand(
        IProductRepository repository,
        ProductCreateDto createDto)
    {
        _repository = repository;
        _createDto = createDto;
    }

    public async Task ExecuteAsync() => await _repository.CreateAsync(_createDto);
}