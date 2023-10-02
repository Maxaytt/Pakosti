using Microsoft.AspNetCore.Mvc;
using Pakosti.Api.BaseControllers;
using Pakosti.Application.Features.Administrator.Identities.Commands;

namespace Pakosti.Api.Areas.Administrator;

public class IdentityController : AdminBaseController
{
    [HttpPost("revoke-all")]
    public async Task<ActionResult> RevokeAll(CancellationToken cancellationToken)
    {
        await Mediator.Send(new RevokeAll.Command(), cancellationToken);
        return Ok();
    }

    [HttpPost("revoke/{id:guid}")]
    public async Task<ActionResult> Revoke(Guid id,
        CancellationToken cancellationToken)
    {
        var command = new Revoke.Command(id);
        await Mediator.Send(command, cancellationToken);
        return Ok();
    }
}