using GestaoProdutos.Application.Commands.Abstraction;
using GestaoProdutos.Domain.Repositories;
using GestaoProdutos.Domain.Repositories.Dtos;

namespace GestaoProdutos.Application.Commands.Factories;

public class CommandFactory : ICommandFactory
{
    private readonly IProductRepository _repository;
    
    public CommandFactory(IProductRepository repository)
    {
        _repository = repository;
    }
    
    public ICommand CreateProductCreateCommand(ProductCreateDto createDto) => 
        new ProductCreateCommand(_repository, createDto);

    public ICommand CreateProductUpdateCommand(int productCode, ProductUpdateDto updateDto) =>
        new ProductUpdateCommand(_repository, productCode, updateDto);

    public ICommand CreateProductDeleteCommand(int productCode) =>
        new ProductDeleteCommand(_repository, productCode);
}