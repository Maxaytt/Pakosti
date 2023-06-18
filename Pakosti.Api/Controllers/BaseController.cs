using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Pakosti.Controllers;

[ApiController]
[Route("[controller]/")]
public class BaseController : Controller
{
    private IMediator? _mediator;

    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetService<IMediator>(); 

    internal Guid UserId => !User.Identity!.IsAuthenticated
        ? Guid.Empty
        : Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
}