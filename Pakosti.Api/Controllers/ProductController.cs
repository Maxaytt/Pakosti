using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Api.Models.Product;
using Pakosti.Application.Features.Products.Commands;
using Pakosti.Application.Features.Products.Queries;

namespace Pakosti.Api.Controllers;


public class ProductController : BaseController
{
    private readonly IMapper _mapper;

    public ProductController(IMapper mapper) =>
        _mapper = mapper;

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var query = new GetProductList.Query();

        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> Get(Guid id)
    {
        var query = new GetProduct.Query(id);

        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateProductDto createProductDto)
    {
        var command = _mapper.Map<CreateProduct.Command>(createProductDto)
            with { UserId = UserId };
        var noteId = await Mediator.Send(command);
        return Ok(noteId);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> Update([FromBody] UpdateProductDto updateProductDto)
    {
        var command = _mapper.Map<UpdateProduct.Command>(updateProductDto)
            with { UserId = UserId };
        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Delete(Guid id)
    {
        var command = new DeleteProduct.Command(id, UserId);
        await Mediator.Send(command);
        return NoContent();
    }
}