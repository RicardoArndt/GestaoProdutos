namespace GestaoProdutos.CrossCutting.Commons;

public class ProductNotFoundException : Exception
{
    public ProductNotFoundException(): base("Product not found") { }
}