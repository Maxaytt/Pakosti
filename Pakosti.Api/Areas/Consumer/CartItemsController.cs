using Mapster;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Api.BaseControllers;
using Pakosti.Application.Features.Consumer.CartItems.Commands;
using Pakosti.Application.Features.Consumer.CartItems.Queries;

namespace Pakosti.Api.Areas.Consumer;

[Route("api/cart/items")]
public class CartItemsController : ConsumerBaseController
{
    [HttpPost]
    public async Task<ActionResult> AddItem([FromBody] AddItem.Dto request,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<AddItem.Command>()
            with { UserId = UserId };
        
        var response = await Mediator.Send(command, cancellationToken);
        return Created($"/api/cart/items/{response.Id}", response);
    }
    
    [HttpPut]
    public async Task<ActionResult> UpdateItem(UpdateItem.Dto request,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<UpdateItem.Command>()
            with { UserId = UserId };
        
        await Mediator.Send(command, cancellationToken);
        return NoContent();
    }
    
    [HttpDelete]
    public async Task<ActionResult> DeleteItem(DeleteItem.Dto request,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<DeleteItem.Command>()
            with { UserId = UserId };

        await Mediator.Send(command, cancellationToken);
        return NoContent();
    }
    
    [HttpGet("{itemId:guid}")]
    public async Task<ActionResult> GetItem(Guid itemId,
        CancellationToken cancellationToken)
    {
        var query = new GetItem.Query(itemId, UserId);

        var response = await Mediator.Send(query, cancellationToken);
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<ActionResult> GetAllItems(CancellationToken cancellationToken)
    {
        var query = new GetItemList.Query(UserId);

        var response = await Mediator.Send(query, cancellationToken);
        return Ok(response.Items);
    }
}