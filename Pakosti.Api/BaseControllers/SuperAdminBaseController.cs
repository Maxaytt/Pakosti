using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Domain.Constants;

namespace Pakosti.Api.BaseControllers;

[Authorize(Roles = RoleConstants.SuperAdministrator)]
[Area(AreaConstants.SuperAdministrator)]
[Route("api/[area]/[controller]")]
public class SuperAdminBaseController : BaseController
{
    
}