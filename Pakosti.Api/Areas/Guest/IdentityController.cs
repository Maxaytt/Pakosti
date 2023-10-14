using Microsoft.AspNetCore.Mvc;
using Pakosti.Api.BaseControllers;
using Pakosti.Application.Features.Guest.Identities.Commands;

namespace Pakosti.Api.Areas.Guest;

public class IdentityController : GuestBaseController
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
    
    [HttpPost("refresh-token")]
    public async Task<ActionResult> RefreshToken(RefreshToken.Command request,
        CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);
        return Ok(response);
    }
}