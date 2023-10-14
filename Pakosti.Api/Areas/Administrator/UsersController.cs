using Microsoft.AspNetCore.Mvc;
using Pakosti.Api.BaseControllers;
using Pakosti.Application.Features.Administrator.Users.Commands;
using Pakosti.Application.Features.Administrator.Users.Queries;

namespace Pakosti.Api.Areas.Administrator;

public class UsersController : AdminBaseController
{
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateUser.Command request, 
        CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);
        return Created($"/api/users/{response.UserId}", response);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateUser.Command request, 
        CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);
        return NoContent();
    }
    
    [HttpDelete("{userId:guid}")]
    public async Task<ActionResult> Delete(Guid userId, 
        CancellationToken cancellationToken)
    {
        var query = new DeleteUser.Command(userId);
            
        await Mediator.Send(query, cancellationToken);
        return NoContent();
    }
    
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult> Get(Guid userId,
        CancellationToken cancellationToken)
    {
        var query = new GetUser.Query(userId);

        var vm = await Mediator.Send(query, cancellationToken);
        return Ok(vm);
    }
    
    [HttpGet]
    public async Task<ActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetUserList.Query();

        var vm = await Mediator.Send(query, cancellationToken);
        return Ok(vm);
    }
}