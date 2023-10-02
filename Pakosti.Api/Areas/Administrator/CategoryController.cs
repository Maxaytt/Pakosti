using Microsoft.AspNetCore.Mvc;
using Pakosti.Api.BaseControllers;
using Pakosti.Application.Features.Administrator.Categories.Commands;
using Pakosti.Application.Features.Administrator.Categories.Queries;

        var vm = await Mediator.Send(query, cancellationToken);
        return Ok(vm);
    }

public class CategoryController : AdminBaseController
{
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateCategory.Command request,
        CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(request, cancellationToken);
        return Created($"/api/admin/category/{response.Id}", response);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateCategory.Command request,
        CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id,
        CancellationToken cancellationToken)
    {
        var query = new DeleteCategory.Command(id);
            
        await Mediator.Send(query, cancellationToken);
        return NoContent();
    }
    
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
}