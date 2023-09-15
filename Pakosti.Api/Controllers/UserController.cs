using Mapster;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Application.Features.Users.Commands;
using Pakosti.Application.Features.Users.Queries;

namespace Pakosti.Api.Controllers;

public class UserController : BaseController
{
    //todo: should be in update method
    [HttpPost("{userId:guid}/assign-role")]
    public async Task<ActionResult> AssignRole(Guid userId, [FromBody] AssignRole.Dto request,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<AssignRole.Command>()
            with {UserId = userId};
        await Mediator.Send(command, cancellationToken);
        return NoContent();
    }
    
    //todo: should be in common get method
    [HttpGet("{userId:guid}/role")]
    public async Task<ActionResult> GetRole(Guid userId,
        CancellationToken cancellationToken)
    {
        var query = new GetRole.Query(userId);
        var response = await Mediator.Send(query, cancellationToken);
        return Ok(response);
    }
    
}