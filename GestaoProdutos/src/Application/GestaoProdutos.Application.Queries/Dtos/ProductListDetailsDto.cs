namespace GestaoProdutos.Application.Queries.Dtos;

public record ProductListDetailsDto
{
    public int TotalRegistries { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public IList<ProductDetailsDto> Results { get; init; }
}