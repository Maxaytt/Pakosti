using Microsoft.AspNetCore.Mvc;
using Pakosti.Api.BaseControllers;
using Pakosti.Application.Features.Administrator.UserRoles.Queries;

namespace Pakosti.Api.Areas.Administrator;

[Route("api/admin/user/roles")]
public class UserRolesController : AdminBaseController
{
    [HttpGet]
    public async Task<IActionResult> GetUserRoles(Guid userId,
        CancellationToken cancellationToken)
    {
        var command = new GetUserRoles.Query(userId);
        var response = await Mediator.Send(command, cancellationToken);
        
        return Ok(response);
    }
}