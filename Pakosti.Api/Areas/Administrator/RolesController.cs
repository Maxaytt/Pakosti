using Microsoft.AspNetCore.Mvc;
using Pakosti.Api.BaseControllers;
using Pakosti.Application.Features.Administrator.Roles.Commands;

namespace Pakosti.Api.Areas.Administrator;

public class RolesController : AdminBaseController
{
    [HttpGet]
    public async Task<ActionResult> GetRoles(CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(new GetRoles.Query(), cancellationToken);
        return Ok(response);
    }
}