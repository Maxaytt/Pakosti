using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Api.Models.Category;
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
    public async Task<ActionResult<Guid>> Create([FromBody] CreateCategoryDto createCategoryDto)
    {
        var query = new CreateCategory.Command(
            createCategoryDto.ParentCategoryId,
            createCategoryDto.Name);

        var id = await Mediator.Send(query);
        return Ok(id);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> Update([FromBody] UpdateCategoryDto updateCategoryDto)
    {
        var query = new UpdateCategory.Command(
            updateCategoryDto.Id,
            updateCategoryDto.ParentCategoryId,
            updateCategoryDto.Name);

        await Mediator.Send(query);
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