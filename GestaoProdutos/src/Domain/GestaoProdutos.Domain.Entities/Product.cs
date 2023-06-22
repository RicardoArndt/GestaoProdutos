namespace GestaoProdutos.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public string Descricao { get; set; }
    public bool Situacao { get; set; }
    public DateTime DataFabricacao { get; set; }
    public DateTime DataValidade { get; set; }
    public Guid FornecedorId { get; set; }

    public virtual Provider Fornecedor { get; set; }
}