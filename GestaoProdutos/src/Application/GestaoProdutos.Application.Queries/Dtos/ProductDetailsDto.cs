namespace GestaoProdutos.Application.Queries.Dtos;

public record ProductDetailsDto
{
    public Guid Id { get; init; }
    public int Codigo { get; init; }
    public string Descricao { get; init; }
    public bool Situacao { get; init; }
    public DateTime DataFabricacao { get; init; }
    public DateTime DataValidade { get; init; }
    public ProviderDetailsDto Provider { get; init; }
}