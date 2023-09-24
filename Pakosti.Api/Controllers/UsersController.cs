using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Application.Features.Users.Commands;
using Pakosti.Application.Features.Users.Queries;

namespace Pakosti.Api.Controllers;

public class UsersController : BaseController
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
    
    [HttpGet]
    public async Task<ActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetUserList.Query();

        var vm = await Mediator.Send(query, cancellationToken);
        return Ok(vm);
    }

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult> Get(Guid userId, CancellationToken cancellationToken)
    {
        var query = new GetUser.Query(userId);

        var vm = await Mediator.Send(query, cancellationToken);
        return Ok(vm);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Create([FromBody] CreateUser.Command request, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);
        return Created($"/api/user/{response.UserId}", response);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> Update([FromBody] UpdateUser.Command request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);
        return NoContent();
    }
    
    [HttpDelete("{userId:guid}")]
    [Authorize]
    public async Task<ActionResult> Delete(Guid userId, CancellationToken cancellationToken)
    {
        var query = new DeleteUser.Command(userId);
            
        await Mediator.Send(query, cancellationToken);
        return NoContent();
    }
}