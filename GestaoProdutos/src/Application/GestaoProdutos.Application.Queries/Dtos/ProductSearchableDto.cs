using System.ComponentModel.DataAnnotations;

namespace GestaoProdutos.Application.Queries.Dtos;

public class ProductSearchableDto
{
    [Required]
    public int Page { get; init; }
    
    [Required]
    public int PageSize { get; init; } 
    public int? Codigo { get; init; } 
    public string? Descricao { get; init; } 
    public DateTime? DataFabricacaoFrom { get; init; } 
    public DateTime? DataFabricacaoTo { get; init; }
    public DateTime? DataValidadeFrom { get; init; } 
    public DateTime? DataValidadeTo { get; init; } 
    public int? FornecedorCodigo { get; init; } 
    public string? FornecedorDescricao { get; init; }
    public string? FornecedorCnpj { get; init; }
}