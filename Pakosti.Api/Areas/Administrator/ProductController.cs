using Mapster;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Api.BaseControllers;
using Pakosti.Application.Features.Administrator.Products.Commands;

        var vm = await Mediator.Send(query, cancellationToken);
        return Ok(vm);
    }

public class ProductController : AdminBaseController
{
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateProduct.Dto dto,
        CancellationToken cancellationToken)
    {
        var command = dto.Adapt<CreateProduct.Command>();
        var response = await Mediator.Send(command, cancellationToken);
        return Created($"/api/product/{response.Id}", response);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateProduct.Dto dto,
        CancellationToken cancellationToken)
    {
        var command = dto.Adapt<UpdateProduct.Command>();
        await Mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteProduct.Command(id);
        await Mediator.Send(command, cancellationToken);
        return NoContent();
    }
}