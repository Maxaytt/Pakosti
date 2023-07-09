using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Application.Cqrs.Categories.Commands.CreateCategory;
using Pakosti.Application.Cqrs.Categories.Commands.DeleteCategory;
using Pakosti.Application.Cqrs.Categories.Commands.UpdateCategory;
using Pakosti.Application.Cqrs.Categories.Queries.GetCategory;
using Pakosti.Application.Cqrs.Categories.Queries.GetCategoryList;
using Pakosti.Api.Models.Category;

namespace Pakosti.Api.Controllers;

public class CategoryController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<CategoryListVm>> GetAll()
    {
        var query = new GetCategoryListQuery();

        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoryVm>> Get(Guid id)
    {
        var query = new GetCategoryQuery
        {
            Id = id
        };

        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateCategoryDto createCategoryDto)
    {
        var query = new CreateCategoryCommand
        {
            Name = createCategoryDto.Name,
            ParentCategoryId = createCategoryDto.ParentCategoryId
        };

        var id = await Mediator.Send(query);
        return Ok(id);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> Update([FromBody] UpdateCategoryDto updateCategoryDto)
    {
        var query = new UpdateCategoryCommand
        {
            Id = updateCategoryDto.Id,
            Name = updateCategoryDto.Name,
            ParentCategoryId = updateCategoryDto.ParentCategoryId
        };

        await Mediator.Send(query);
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Delete(Guid id)
    {
        var query = new DeleteCategoryCommand
        {
            Id = id
        };

        await Mediator.Send(query);
        return NoContent();
    }
}