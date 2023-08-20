using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Application.Features.Identities.Commands;

namespace Pakosti.Api.Controllers;

public class IdentityController : BaseController
{
    [HttpPost("login")]
    public async Task<ActionResult> Authenticate([FromBody] Authenticate.Command request,
        CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] Register.Command request,
        CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    
    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshToken.Command request,
        CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    [Authorize]
    [HttpPost]
    [Route("revoke/{id:guid}")]
    public async Task<IActionResult> Revoke(Guid id,
        CancellationToken cancellationToken)
    {
        var command = new Revoke.Command(id);
        await Mediator.Send(command, cancellationToken);
        return Ok();
    }
    
    [Authorize(Policy = "AdministratorOnly")]
    [HttpPost]
    [Route("revoke-all")]
    public async Task<IActionResult> RevokeAll(CancellationToken cancellationToken)
    {
        await Mediator.Send(new RevokeAll.Command(), cancellationToken);
        return Ok();
    }
}