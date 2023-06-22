using System.ComponentModel.DataAnnotations;

namespace GestaoProdutos.Domain.Repositories.Dtos;

public record ProductUpdateDto : IValidatableObject
{
    public string Descricao { get; init; }
    public bool Situacao { get; init; }
    public DateTime DataFabricacao { get; init; }
    public DateTime DataValidade { get; init; }
    

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DataFabricacao >= DataValidade)
        {
            yield return new ValidationResult("A Data de Fabricação deve ser menor que a Data de Validade.", new[] { nameof(DataFabricacao), nameof(DataValidade) });
        }
    }
}