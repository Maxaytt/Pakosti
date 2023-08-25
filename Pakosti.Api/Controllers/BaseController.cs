using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Pakosti.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController : Controller
{
    private IMediator? _mediator;

    protected IMediator Mediator => _mediator 
        ??= HttpContext.RequestServices.GetService<IMediator>()!; 

    protected Guid UserId
    {
        get
        {
            if (User.Identity?.IsAuthenticated != true) return Guid.Empty;
            
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
                return userId;

            return Guid.Empty;
        }
    }
}