using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Application.Features.Products.Commands;
using Pakosti.Application.Features.Products.Queries;

namespace Pakosti.Api.Controllers;


public class ProductController : BaseController
{
    [HttpGet]
    public async Task<ActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetProductList.Query();

        var vm = await Mediator.Send(query, cancellationToken);
        return Ok(vm);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> Get(Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetProduct.Query(id);
        var vm = await Mediator.Send(query, cancellationToken);
        return Ok(vm);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Create([FromBody] CreateProduct.Dto createProductDto,
        CancellationToken cancellationToken)
    {
        var command = createProductDto.Adapt<CreateProduct.Command>()
            with { UserId = UserId };
        var response = await Mediator.Send(command, cancellationToken);
        return Created($"/api/product/{response.Id}", response);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> Update([FromBody] UpdateProduct.Dto updateProductDto,
        CancellationToken cancellationToken)
    {
        var command = updateProductDto.Adapt<UpdateProduct.Command>()
            with { UserId = UserId };
        await Mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Delete(Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteProduct.Command(id, UserId);
        await Mediator.Send(command, cancellationToken);
        return NoContent();
    }
}