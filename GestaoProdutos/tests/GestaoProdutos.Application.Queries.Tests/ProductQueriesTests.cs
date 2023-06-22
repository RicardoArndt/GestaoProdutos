using AutoMapper;
using FluentAssertions;
using GestaoProdutos.Application.Queries.Dtos;
using GestaoProdutos.Application.Queries.Tests.Fixtures;
using GestaoProdutos.Domain.Entities;
using GestaoProdutos.Infra.Database.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace GestaoProdutos.Application.Queries.Tests;

public class ProductQueriesTests : IClassFixture<MapperFixture>
{
    private readonly Mock<IContext> _context;
    private readonly ProductQueries _queries;
    
    public ProductQueriesTests(MapperFixture fixture)
    {
        var provider = fixture.BuildServiceProvider();
        _context = new Mock<IContext>();
        _queries = new ProductQueries(_context.Object, provider.GetService<IMapper>()!);
    }
    
    [Fact]
    public void GetProductByIdAsync_ShouldBeReturn_MappedValues()
    {
        var productId = Guid.NewGuid();
        var fornecedorId = Guid.NewGuid();
        
        _context
            .Setup(c => c.QueryAsync(
                "SELECT TOP(1) * FROM Produtos AS p " +
                "JOIN Fornecedores AS f ON f.Id = p.FornecedorId " +
                "WHERE p.Id = @Id",
                It.IsAny<Func<Product, Provider, Product>>(), 
                It.Is<object>(o => o.GetType().GetProperty("Id").GetValue(o).ToString() == productId.ToString()), 
                "Id"))
            .ReturnsAsync(new List<Product>
            {
                new()
                {
                    Id = productId,
                    Codigo = 1,
                    Descricao = "Guaraná",
                    DataFabricacao = DateTime.Now,
                    DataValidade = DateTime.Now.AddMonths(1),
                    Situacao = true,
                    Fornecedor = new()
                    {
                        Cnpj = "12345678912345",
                        Descricao = "Ricardo LTDA",
                        Codigo = 1,
                        Id = fornecedorId
                    }
                }    
            });

        var product = _queries.GetProductByIdAsync(productId).Result;

        product!.Id.Should().Be(productId);
        product.Descricao.Should().Be("Guaraná");
        product.Codigo.Should().Be(1);
        product.DataFabricacao.Date.Should().Be(DateTime.Now.Date);
        product.DataValidade.Date.Should().Be(DateTime.Now.AddMonths(1).Date);
        product.Situacao.Should().BeTrue();
        product.Provider.Id.Should().Be(fornecedorId);
        product.Provider.Descricao.Should().Be("Ricardo LTDA");
        product.Provider.Codigo.Should().Be(1);
        product.Provider.Cnpj.Should().Be("12345678912345");
    }
    
    [Fact]
    public void GetProductByCodeAsync_ShouldBeReturn_MappedValues()
    {
        var productId = Guid.NewGuid();
        var fornecedorId = Guid.NewGuid();
        
        _context
            .Setup(c => c.QueryAsync(
                "SELECT TOP(1) * FROM Produtos AS p " +
                "JOIN Fornecedores AS f ON f.Id = p.FornecedorId " +
                "WHERE p.Codigo = @Code",
                It.IsAny<Func<Product, Provider, Product>>(), 
                It.Is<object>(o => int.Parse(o.GetType().GetProperty("Code").GetValue(o).ToString()) == 1), 
                "Id"))
            .ReturnsAsync(new List<Product>
            {
                new()
                {
                    Id = productId,
                    Codigo = 1,
                    Descricao = "Guaraná",
                    DataFabricacao = DateTime.Now,
                    DataValidade = DateTime.Now.AddMonths(1),
                    Situacao = true,
                    Fornecedor = new()
                    {
                        Cnpj = "12345678912345",
                        Descricao = "Ricardo LTDA",
                        Codigo = 1,
                        Id = fornecedorId
                    }
                }    
            });

        var product = _queries.GetProductByCodeAsync(1).Result;

        product!.Id.Should().Be(productId);
        product.Descricao.Should().Be("Guaraná");
        product.Codigo.Should().Be(1);
        product.DataFabricacao.Date.Should().Be(DateTime.Now.Date);
        product.DataValidade.Date.Should().Be(DateTime.Now.AddMonths(1).Date);
        product.Situacao.Should().BeTrue();
        product.Provider.Id.Should().Be(fornecedorId);
        product.Provider.Descricao.Should().Be("Ricardo LTDA");
        product.Provider.Codigo.Should().Be(1);
        product.Provider.Cnpj.Should().Be("12345678912345");
    }

    [Fact]
    public void GetProductsPagedAsync_ShouldBeReturn_MappedValues()
    {
        var productId = Guid.NewGuid();
        var fornecedorId = Guid.NewGuid();
        
        _context
            .Setup(c => c.QueryAsync(
                "SELECT * FROM Produtos p " +
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
                "OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY",
                It.IsAny<Func<Product, Provider, Product>>(), 
                It.Is<object>(o => int.Parse(o.GetType().GetProperty("Offset").GetValue(o).ToString()) == 0), 
                "Id"))
            .ReturnsAsync(new List<Product>
            {
                new()
                {
                    Id = productId,
                    Codigo = 1,
                    Descricao = "Guaraná",
                    DataFabricacao = DateTime.Now,
                    DataValidade = DateTime.Now.AddMonths(1),
                    Situacao = true,
                    Fornecedor = new()
                    {
                        Cnpj = "12345678912345",
                        Descricao = "Ricardo LTDA",
                        Codigo = 1,
                        Id = fornecedorId
                    }
                }    
            });
        
        _context
            .Setup(c => c.QueryFirstOrDefaultAsync<int>(
                "SELECT COUNT(*) FROM Produtos p " +
                "JOIN Fornecedores f ON p.FornecedorId = f.Id " +
                "WHERE (@Codigo IS NULL OR p.Codigo = @Codigo) " +
                "AND (@Descricao IS NULL OR p.Descricao LIKE @Descricao) " +
                "AND (@DataFabricacaoFrom IS NULL OR p.DataFabricacao >= @DataFabricacaoFrom) " +
                "AND (@DataFabricacaoTo IS NULL OR p.DataFabricacao <= @DataFabricacaoTo) " +
                "AND (@DataValidadeFrom IS NULL OR p.DataValidade >= @DataValidadeFrom) " +
                "AND (@DataValidadeTo IS NULL OR p.DataValidade <= @DataValidadeTo) " +
                "AND (@FornecedorCodigo IS NULL OR f.Codigo = @FornecedorCodigo) " +
                "AND (@FornecedorDescricao IS NULL OR f.Descricao LIKE @FornecedorDescricao) " +
                "AND (@FornecedorCnpj IS NULL OR f.Cnpj = @FornecedorCnpj)",
                It.IsAny<object?>()))
            .ReturnsAsync(1);
        
        var result = _queries.GetProductsPagedAsync(new ProductSearchableDto
        {
            Page = 1,
            PageSize = 3
        }).Result;

        result.CurrentPage.Should().Be(1);
        result.TotalPages.Should().Be(1);
        result.TotalRegistries.Should().Be(1);
        result.Results.Should().SatisfyRespectively(product =>
        {
            product!.Id.Should().Be(productId);
            product.Descricao.Should().Be("Guaraná");
            product.Codigo.Should().Be(1);
            product.DataFabricacao.Date.Should().Be(DateTime.Now.Date);
            product.DataValidade.Date.Should().Be(DateTime.Now.AddMonths(1).Date);
            product.Situacao.Should().BeTrue();
            product.Provider.Id.Should().Be(fornecedorId);
            product.Provider.Descricao.Should().Be("Ricardo LTDA");
            product.Provider.Codigo.Should().Be(1);
            product.Provider.Cnpj.Should().Be("12345678912345");
        });
    }
}