namespace GestaoProdutos.Domain.Entities;

public class Provider
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public string Descricao { get; set; }
    public string Cnpj { get; set; }
}