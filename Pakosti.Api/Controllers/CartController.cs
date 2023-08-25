using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Application.Features.Carts.Commands;
using Pakosti.Application.Features.Carts.Queries;

namespace Pakosti.Api.Controllers;

public class CartController : BaseController
{
    [HttpPost("items")]
    [Authorize]
    public async Task<ActionResult> Add([FromBody] AddItem.Dto request,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<AddItem.Command>()
            with { UserId = UserId };
        
        var response = await Mediator.Send(command, cancellationToken);
        return Created($"/api/cart/items/{response.Id}", response);
    }
    
    [HttpPut("items")]
    [Authorize]
    public async Task<ActionResult> Update(UpdateItem.Dto request,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<UpdateItem.Command>()
            with { UserId = UserId };
        
        await Mediator.Send(command, cancellationToken);
        return NoContent();
    }

    
    [HttpDelete("items")]
    [Authorize]
    public async Task<ActionResult> Delete(DeleteItem.Dto request,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<DeleteItem.Command>()
            with { UserId = UserId };

        await Mediator.Send(command, cancellationToken);
        return NoContent();
    }
    
    [HttpGet("items/{itemId:guid}")]
    [Authorize]
    public async Task<ActionResult> Get(Guid itemId,
        CancellationToken cancellationToken)
    {
        var query = new GetItem.Query(itemId, UserId);

        var response = await Mediator.Send(query, cancellationToken);
        return Ok(response);
    }
    
    [HttpGet("items")]
    [Authorize]
    public async Task<ActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetItemList.Query(UserId);

        var response = await Mediator.Send(query, cancellationToken);
        return Ok(response.Items);
    }
}