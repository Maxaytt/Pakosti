using Mapster;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Api.BaseControllers;
using Pakosti.Application.Features.Consumer.User.Commands;
using Pakosti.Application.Features.Consumer.User.Queries;

namespace Pakosti.Api.Areas.Consumer;

public class UserController : ConsumerBaseController
{
    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateUser.Dto request, 
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<UpdateUser.Command>()
            with { UserId = UserId };
        
        await Mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult> Get(CancellationToken cancellationToken)
    {
        var query = new GetUser.Query(UserId);

        var response = await Mediator.Send(query, cancellationToken);
        return Ok(response);
    }
}