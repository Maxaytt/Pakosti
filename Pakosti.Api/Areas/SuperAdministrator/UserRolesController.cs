using Mapster;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Api.BaseControllers;
using Pakosti.Application.Features.SuperAdministrator.UserRoles.Commands;

namespace Pakosti.Api.Areas.SuperAdministrator;

[Route("api/users/{userId:guid}/roles")]
public class UserRolesController : SuperAdminBaseController
{
    [HttpPost]
    public async Task<IActionResult> AddRoleToUser(Guid userId, [FromBody] AddRoleToUser.Dto request,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<AddRoleToUser.Command>()
            with { UserId = userId };
        await Mediator.Send(command, cancellationToken);
        
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveRoleFromUser(Guid userId, [FromBody] RemoveRoleFromUser.Dto request,
        CancellationToken cancellationToken)
    {
        var command = request.Adapt<RemoveRoleFromUser.Command>()
            with { UserId = userId };
        await Mediator.Send(command, cancellationToken);
        
        return NoContent();
    }
}