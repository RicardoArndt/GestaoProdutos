using GestaoProdutos.Application.Commands.Abstraction;
using GestaoProdutos.Application.Queries.Abstraction;
using GestaoProdutos.Application.Queries.Dtos;
using GestaoProdutos.CrossCutting.Commons;
using GestaoProdutos.Domain.Repositories.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace GestaoProdutos.Presenter.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : Controller
{
    private readonly IProductQueries _productQueries;
    private readonly ICommandFactory _commandFactory;
    
    public ProductsController(
        IProductQueries productQueries,
        ICommandFactory commandFactory)
    {
        _productQueries = productQueries;
        _commandFactory = commandFactory;
    }

    [HttpGet("{productCode}", Name = "GetProductByCodeAsync")]
    public async Task<IActionResult> GetProductByCodeAsync([FromRoute] int productCode)
    {
        try
        {
            return Ok(await _productQueries.GetProductByCodeAsync(productCode));
        }
        catch (ProductNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetProductsPagedAsync([FromQuery] ProductSearchableDto productSearchableDto)
    {
        return Ok(await _productQueries.GetProductsPagedAsync(productSearchableDto));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] ProductCreateDto createDto)
    {
        var command = _commandFactory.CreateProductCreateCommand(createDto);
        
        await command.ExecuteAsync();

        var product = await _productQueries.GetProductByIdAsync(createDto.GetProductId());
        return CreatedAtAction("GetProductByCodeAsync", new { productCode = product.Codigo }, createDto);
    }
    
    [HttpPut("{productCode}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int productCode, [FromBody] ProductUpdateDto updateDto)
    {
        try
        {
            var command = _commandFactory.CreateProductUpdateCommand(productCode, updateDto);
        
            await command.ExecuteAsync();

            return Ok();
        }
        catch (ProductNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpDelete("{productCode}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int productCode)
    {
        try
        {
            var command = _commandFactory.CreateProductDeleteCommand(productCode);
        
            await command.ExecuteAsync();

            return Ok();
        }
        catch (ProductNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}