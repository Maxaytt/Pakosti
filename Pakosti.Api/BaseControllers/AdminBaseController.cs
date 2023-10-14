using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Domain.Constants;

namespace Pakosti.Api.BaseControllers;

[Authorize(Roles = RoleConstants.Administrator)]
[Area(AreaConstants.Administrator)]
[Route("api/[area]/[controller]")]
public class AdminBaseController : BaseController
{
    
}