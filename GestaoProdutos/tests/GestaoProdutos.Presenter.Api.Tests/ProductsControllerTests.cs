using FluentAssertions;
using GestaoProdutos.Application.Commands.Abstraction;
using GestaoProdutos.Application.Queries.Abstraction;
using GestaoProdutos.Application.Queries.Dtos;
using GestaoProdutos.CrossCutting.Commons;
using GestaoProdutos.Presenter.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GestaoProdutos.Presenter.Api.Tests;

public class ProductsControllerTests
{
    private readonly Mock<IProductQueries> _queries;
    private readonly Mock<ICommandFactory> _commandFactory;
    private readonly ProductsController _controller;
    
    public ProductsControllerTests()
    {
        _queries = new Mock<IProductQueries>();
        _commandFactory = new Mock<ICommandFactory>();
        _controller = new ProductsController(
            _queries.Object, 
            _commandFactory.Object);
    }

    [Fact]
    public void GetProductByCodeAsync_ShouldBeOfType_OkObjectResult()
    {
        var result = _controller.GetProductByCodeAsync(1).Result;
        result.Should().BeOfType<OkObjectResult>();
    }
    
    [Fact]
    public void GetProductByCodeAsync_ShouldBeOfType_NotFoundObjectResult()
    {
        _queries.Setup(q => q.GetProductByCodeAsync(1)).ThrowsAsync(new ProductNotFoundException());
        
        var result = _controller.GetProductByCodeAsync(1).Result;
        result.Should().BeOfType<NotFoundObjectResult>();
    }
    
    [Fact]
    public void GetProductsPagedAsync_ShouldBeOfType_NotFoundObjectResult()
    {
        var result = _controller.GetProductsPagedAsync(new ProductSearchableDto
        {
            Page = 1,
            PageSize = 1
        }).Result;
        result.Should().BeOfType<OkObjectResult>();
    }
}