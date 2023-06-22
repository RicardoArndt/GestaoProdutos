using AutoMapper;
using GestaoProdutos.Application.Queries.Abstraction;
using GestaoProdutos.Application.Queries.Dtos;
using GestaoProdutos.CrossCutting.Commons;
using GestaoProdutos.Domain.Entities;
using GestaoProdutos.Infra.Database.Abstraction;

namespace GestaoProdutos.Application.Queries;

internal sealed class ProductQueries : IProductQueries
{
    private readonly IContext _context;
    private readonly IMapper _mapper;
    
    public ProductQueries(IContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductDetailsDto> GetProductByIdAsync(Guid id)
    {
        var query = "SELECT TOP(1) * FROM Produtos AS p " +
                    "JOIN Fornecedores AS f ON f.Id = p.FornecedorId " +
                    "WHERE p.Id = @Id";

        var result = (await _context.QueryAsync<Product, Provider, Product>(query, (product, provider) =>
        {
            product.Fornecedor = provider;
            return product;
        }, new { Id = id })).FirstOrDefault();

        if (result is null)
            throw new ProductNotFoundException();

        return _mapper.Map<Product, ProductDetailsDto>(result);
    }

    public async Task<ProductDetailsDto> GetProductByCodeAsync(int code)
    {
        var query = "SELECT TOP(1) * FROM Produtos AS p " +
                    "JOIN Fornecedores AS f ON f.Id = p.FornecedorId " +
                    "WHERE p.Codigo = @Code";

        var result = (await _context.QueryAsync<Product, Provider, Product>(query, (product, provider) =>
        {
            product.Fornecedor = provider;
            return product;
        }, new { Code = code })).FirstOrDefault();

        if (result is null)
            throw new ProductNotFoundException();

        return _mapper.Map<Product, ProductDetailsDto>(result);
    }
    
    public async Task<ProductListDetailsDto> GetProductsPagedAsync(ProductSearchableDto productSearchableDto)
    {
        var query = "SELECT * FROM Produtos p " +
                    "JOIN Fornecedores f ON p.FornecedorId = f.Id " +
                    "WHERE (@Codigo IS NULL OR p.Codigo = @Codigo) " +
                    "AND (@Descricao IS NULL OR p.Descricao LIKE @Descricao) " +
                    "AND (@DataFabricacaoFrom IS NULL OR p.DataFabricacao >= @DataFabricacaoFrom) " +
                    "AND (@DataFabricacaoTo IS NULL OR p.DataFabricacao <= @DataFabricacaoTo) " +
                    "AND (@DataValidadeFrom IS NULL OR p.DataValidade >= @DataValidadeFrom) " +
                    "AND (@DataValidadeTo IS NULL OR p.DataValidade <= @DataValidadeTo) " +
                    "AND (@FornecedorCodigo IS NULL OR f.Codigo = @FornecedorCodigo) " +
                    "AND (@FornecedorDescricao IS NULL OR f.Descricao LIKE @FornecedorDescricao) " +
                    "AND (@FornecedorCnpj IS NULL OR f.Cnpj = @FornecedorCnpj) " +
                    "ORDER BY p.Codigo " +
                    "OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        var parameters = new
        {
            productSearchableDto.Codigo,
            Descricao = "%" + productSearchableDto.Descricao + "%",
            productSearchableDto.DataFabricacaoFrom,
            productSearchableDto.DataFabricacaoTo,
            productSearchableDto.DataValidadeFrom,
            productSearchableDto.DataValidadeTo,
            productSearchableDto.FornecedorCodigo,
            FornecedorDescricao = "%" + productSearchableDto.FornecedorDescricao + "%",
            productSearchableDto.FornecedorCnpj,
            Offset = (productSearchableDto.Page - 1) * productSearchableDto.PageSize,
            productSearchableDto.PageSize
        };

        var products = (await _context.QueryAsync<Product, Provider, Product>(query,
            (product, provider) =>
            {
                product.Fornecedor = provider;
                return product;
            },
            parameters,
            splitOn: "Id"))
            .Distinct()
            .ToList();
        
        var countQuery = "SELECT COUNT(*) FROM Produtos p " +
                         "JOIN Fornecedores f ON p.FornecedorId = f.Id " +
                         "WHERE (@Codigo IS NULL OR p.Codigo = @Codigo) " +
                         "AND (@Descricao IS NULL OR p.Descricao LIKE @Descricao) " +
                         "AND (@DataFabricacaoFrom IS NULL OR p.DataFabricacao >= @DataFabricacaoFrom) " +
                         "AND (@DataFabricacaoTo IS NULL OR p.DataFabricacao <= @DataFabricacaoTo) " +
                         "AND (@DataValidadeFrom IS NULL OR p.DataValidade >= @DataValidadeFrom) " +
                         "AND (@DataValidadeTo IS NULL OR p.DataValidade <= @DataValidadeTo) " +
                         "AND (@FornecedorCodigo IS NULL OR f.Codigo = @FornecedorCodigo) " +
                         "AND (@FornecedorDescricao IS NULL OR f.Descricao LIKE @FornecedorDescricao) " +
                         "AND (@FornecedorCnpj IS NULL OR f.Cnpj = @FornecedorCnpj)";

        var count = await _context.QueryFirstOrDefaultAsync<int>(countQuery, parameters);

        var totalRegistries = count;
        var totalPages = (int)Math.Ceiling((double)count / productSearchableDto.PageSize);
        var currentPage = productSearchableDto.Page;

        return new ProductListDetailsDto
        {
            CurrentPage = currentPage,
            TotalRegistries = totalRegistries,
            TotalPages = totalPages,
            Results = _mapper.Map<List<Product>, List<ProductDetailsDto>>(products)
        };
    }
}