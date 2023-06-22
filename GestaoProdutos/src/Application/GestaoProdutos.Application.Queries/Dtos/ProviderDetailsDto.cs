namespace GestaoProdutos.Application.Queries.Dtos;

public record ProviderDetailsDto
{
    public Guid Id { get; init; }
    public int Codigo { get; init; }
    public string Descricao { get; init; }
    public string Cnpj { get; init; }
}