using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Application.Features.Categories.Commands;
using Pakosti.Application.Features.Categories.Queries;

namespace Pakosti.Api.Controllers;

public class CategoryController : BaseController
{
    [HttpGet]
    public async Task<ActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetCategoryList.Query();

        var vm = await Mediator.Send(query, cancellationToken);
        return Ok(vm);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> Get(Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetCategory.Query(id);

        var vm = await Mediator.Send(query, cancellationToken);
        return Ok(vm);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Create([FromBody] CreateCategory.Command request,
        CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);
        return Created($"/api/category/{response.Id}", response);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> Update([FromBody] UpdateCategory.Command request,
        CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Delete(Guid id,
        CancellationToken cancellationToken)
    {
        var query = new DeleteCategory.Command(id);
            
        await Mediator.Send(query, cancellationToken);
        return NoContent();
    }
}