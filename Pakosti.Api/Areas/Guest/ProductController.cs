using Microsoft.AspNetCore.Mvc;
using Pakosti.Api.BaseControllers;
using Pakosti.Application.Features.Guest.Products.Queries;

namespace Pakosti.Api.Areas.Guest;

public class ProductController : GuestBaseController
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> Get(Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetProduct.Query(id);
        var vm = await Mediator.Send(query, cancellationToken);
        return Ok(vm);
    }
    
    [HttpGet]
    public async Task<ActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetProductList.Query();

        var vm = await Mediator.Send(query, cancellationToken);
        return Ok(vm);
    }
}