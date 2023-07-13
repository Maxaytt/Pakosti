using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Application.Features.Identities.Commands;

namespace Pakosti.Api.Controllers;

public class IdentityController : BaseController
{
    [HttpPost("login")]
    public async Task<ActionResult> Authenticate([FromBody] Authenticate.Command request)
    {
        var response = await Mediator.Send(request);
        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] Register.Command request)
    {
        var response = await Mediator.Send(request);
        return Ok(response);
    }
    
    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshToken.Command request)
    {
        var response = await Mediator.Send(request);
        return Ok(response);
    }
    [Authorize]
    [HttpPost]
    [Route("revoke/{username}")]
    public async Task<IActionResult> Revoke(string username)
    {
        var command = new Revoke.Command(username);
        await Mediator.Send(command);
        return Ok();
    }
    
    [Authorize(Policy = "AdministratorOnly")]
    [HttpPost]
    [Route("revoke-all")]
    public async Task<IActionResult> RevokeAll()
    {
        await Mediator.Send(new RevokeAll.Command());
        return Ok();
    }
}