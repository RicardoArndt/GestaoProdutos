using GestaoProdutos.Domain.Repositories.Dtos;

namespace GestaoProdutos.Application.Commands.Abstraction;

public interface ICommandFactory
{
    ICommand CreateProductCreateCommand(ProductCreateDto createDto);
    ICommand CreateProductUpdateCommand(int productCode, ProductUpdateDto updateDto);
    ICommand CreateProductDeleteCommand(int productCode);
}