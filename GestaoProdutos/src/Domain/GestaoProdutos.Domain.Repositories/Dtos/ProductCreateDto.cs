using System.ComponentModel.DataAnnotations;

namespace GestaoProdutos.Domain.Repositories.Dtos;

public record ProductCreateDto : IValidatableObject
{
    private readonly Guid _createdProductId;

    public ProductCreateDto()
    {
        _createdProductId = Guid.NewGuid();
    }

    [Required]
    public string Descricao { get; init; }
    public bool Situacao { get; init; }
    
    [Required]
    public DateTime DataFabricacao { get; init; }
    
    [Required]
    public DateTime DataValidade { get; init; }
    
    [Required]
    public string DescricaoFornecedor { get; init; }
    
    [Required]
    public string CnpjFornecedor { get; init; }

    public Guid GetProductId()
    {
        return _createdProductId;
    }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DataFabricacao >= DataValidade)
        {
            yield return new ValidationResult("A Data de Fabricação deve ser menor que a Data de Validade.", new[] { nameof(DataFabricacao), nameof(DataValidade) });
        }
    }
}