using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Application.Features.Products.Commands.CreateProduct;
using Pakosti.Application.Features.Products.Commands.DeleteProduct;
using Pakosti.Application.Features.Products.Commands.UpdateProduct;
using Pakosti.Application.Features.Products.Queries.GetProduct;
using Pakosti.Application.Features.Products.Queries.GetProductList;
using Pakosti.Api.Models.Product;

namespace Pakosti.Api.Controllers;


public class ProductController : BaseController
{
    private readonly IMapper _mapper;

    public ProductController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<ProductListVm>> GetAll()
    {
        var query = new GetProductListQuery();

        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductVm>> Get(Guid id)
    {
        var query = new GetProductQuery
        {
            Id = id,
        };

        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateProductDto createProductDto)
    {
        var command = _mapper.Map<CreateProductCommand>(createProductDto);
        command.UserId = UserId;
        var noteId = await Mediator.Send(command);
        return Ok(noteId);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> Update([FromBody] UpdateProductDto updateProductDto)
    {
        var command = _mapper.Map<UpdateProductCommand>(updateProductDto);
        command.UserId = UserId;
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Delete(Guid id)
    {
        var command = new DeleteProductCommand
        {
            Id = id,
            UserId = UserId
        };
        await Mediator.Send(command);
        return NoContent();
    }
}