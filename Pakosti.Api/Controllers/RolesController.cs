using Microsoft.AspNetCore.Mvc;
using Pakosti.Application.Features.Roles.Commands;

namespace Pakosti.Api.Controllers;

public class RolesController : BaseController
{
    [HttpGet]
    public async Task<ActionResult> GetRoles(CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new GetRoles.Query(), cancellationToken);
        return Ok(response);
    }
}