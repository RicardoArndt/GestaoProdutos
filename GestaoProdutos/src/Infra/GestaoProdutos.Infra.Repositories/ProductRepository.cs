using GestaoProdutos.CrossCutting.Commons;
using GestaoProdutos.Domain.Entities;
using GestaoProdutos.Domain.Repositories;
using GestaoProdutos.Domain.Repositories.Dtos;
using GestaoProdutos.Infra.Database.Abstraction;

namespace GestaoProdutos.Infra.Repositories;

internal sealed class ProductRepository : IProductRepository
{
    private readonly IContext _context;
    
    public ProductRepository(IContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(ProductCreateDto createDto)
    {
        var existingProviderId = await _context.QueryFirstOrDefaultAsync<Guid?>(
            "SELECT Id FROM Fornecedores WHERE Cnpj = @Cnpj",
            new { Cnpj = createDto.CnpjFornecedor });
        
        var product = new Product
        {
            Id = createDto.GetProductId(),
            Descricao = createDto.Descricao,
            Situacao = createDto.Situacao,
            DataFabricacao = createDto.DataFabricacao,
            DataValidade = createDto.DataValidade,
            FornecedorId = existingProviderId ?? Guid.Empty
        };
        
        if (existingProviderId is null)
        {
            var provider = new Provider
            {
                Id = Guid.NewGuid(),
                Descricao = createDto.DescricaoFornecedor,
                Cnpj = createDto.CnpjFornecedor
            };
            
            var insertProviderQuery = "INSERT INTO Fornecedores (Id, Descricao, Cnpj) " +
                                      "VALUES (@Id, @Descricao, @Cnpj)";
            
            await _context.ExecuteAsync(insertProviderQuery, provider);

            product.FornecedorId = provider.Id;
        }

        var insertProductQuery = "INSERT INTO Produtos (Id, Descricao, Situacao, DataFabricacao, DataValidade, FornecedorId) " +
                                 "VALUES (@Id, @Descricao, @Situacao, @DataFabricacao, @DataValidade, @FornecedorId)";
        
        await _context.ExecuteAsync(insertProductQuery, product);
    }

    public async Task UpdateAsync(int productCode, ProductUpdateDto updateDto)
    {
        var product = await GetProductAndValidateByCodeAsync(productCode);

        var updateQuery = @"UPDATE Produtos
              SET Descricao = @Descricao,
                  Situacao = @Situacao,
                  DataFabricacao = @DataFabricacao,
                  DataValidade = @DataValidade
              WHERE Id = @ProductId";
        
        await _context.ExecuteAsync(updateQuery,
            new
            {
                updateDto.Descricao,
                updateDto.Situacao,
                updateDto.DataFabricacao,
                updateDto.DataValidade,
                ProductId = product.Id
            });
    }

    public async Task DeleteAsync(int productCode)
    {
        var product = await GetProductAndValidateByCodeAsync(productCode);

        var deleteQuery = "DELETE Produtos WHERE Id = @ProductId";
        await _context.ExecuteAsync(
            deleteQuery, 
            new
            {   
                ProductId = product.Id
            });
    }

    private async Task<Product> GetProductAndValidateByCodeAsync(int productCode)
    {
        var product = await _context.QueryFirstOrDefaultAsync<Product?>(
            "SELECT * FROM Produtos WHERE Codigo = @Code",
            new { Code = productCode });

        if (product is null)
            throw new ProductNotFoundException();

        return product;
    }
}