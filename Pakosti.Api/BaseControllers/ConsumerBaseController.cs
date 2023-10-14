using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Domain.Constants;

namespace Pakosti.Api.BaseControllers;

[Authorize(Roles = RoleConstants.Consumer)]
[Area(AreaConstants.Consumer)]
public class ConsumerBaseController : BaseController
{
    
}