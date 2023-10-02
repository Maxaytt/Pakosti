using Microsoft.AspNetCore.Mvc;
using Pakosti.Api.BaseControllers;
using Pakosti.Application.Features.Consumer.Identities.Commands;

namespace Pakosti.Api.Areas.Consumer;

public class IdentityController : ConsumerBaseController
{
    [HttpPost("revoke")]
    public async Task<ActionResult> Revoke(CancellationToken cancellationToken)
    {
        var command = new Revoke.Command(UserId);

        await Mediator.Send(command, cancellationToken);
        return NoContent();
    }
}