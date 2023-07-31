using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Application.Features.Products.Commands;
using Pakosti.Application.Features.Products.Queries;

namespace Pakosti.Api.Controllers;


public class ProductController : BaseController
{
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
    public async Task<ActionResult<Guid>> Create([FromBody] CreateProduct.Dto createProductDto)
    {
        var command = createProductDto.Adapt<CreateProduct.Command>()
            with { UserId = UserId };
        var noteId = await Mediator.Send(command);
        return Ok(noteId);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> Update([FromBody] UpdateProduct.Dto updateProductDto)
    {
        var command = updateProductDto.Adapt<UpdateProduct.Command>()
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