namespace GestaoProdutos.Application.Commands.Abstraction;

public interface ICommand
{
    Task ExecuteAsync();
}