using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Application.Features.Categories.Commands;
using Pakosti.Application.Features.Categories.Queries;

namespace Pakosti.Api.Controllers;

public class CategoryController : BaseController
{
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var query = new GetCategoryList.Query();

        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> Get(Guid id)
    {
        var query = new GetCategory.Query(id);

        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Create([FromBody] CreateCategory.Command request)
    {
        var response = await Mediator.Send(request);
        return Created($"/api/category/{response.Id}", response);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> Update([FromBody] UpdateCategory.Command request)
    {
        await Mediator.Send(request);
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Delete(Guid id)
    {
        var query = new DeleteCategory.Command(id);
            
        await Mediator.Send(query);
        return NoContent();
    }
}