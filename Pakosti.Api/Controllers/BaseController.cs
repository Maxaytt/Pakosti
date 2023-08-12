using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Pakosti.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController : Controller
{
    private IMediator? _mediator;

    protected IMediator? Mediator => _mediator 
        ??= HttpContext.RequestServices.GetService<IMediator>(); 

    internal Guid UserId => !User.Identity?.IsAuthenticated ?? false
        ? Guid.Empty
        : Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                     ?? Guid.Empty.ToString());
}