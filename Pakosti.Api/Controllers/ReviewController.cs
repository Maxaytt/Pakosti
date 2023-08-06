using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Application.Features.Reviews.Commands;
using Pakosti.Application.Features.Reviews.Queries;

namespace Pakosti.Api.Controllers;

public class ReviewController : BaseController
{
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var query = new GetReviewList.Query();

        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> Get(Guid id)
    {
        var query = new GetReview.Query(id);

        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Crete([FromBody] CreateReview.Dto createReviewDto)
    {
        var query = createReviewDto.Adapt<CreateReview.Command>()
            with { UserId = UserId };
        var response = await Mediator.Send(query);
        return Created($"/api/review/{response.Id}", response);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> Update([FromBody] UpdateReview.Dto updateReviewDto)
    {
        var query = updateReviewDto.Adapt<UpdateReview.Command>()
            with { UserId = UserId };
        await Mediator.Send(query);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Delete(Guid id)
    {
        var query = new DeleteReview.Command(id, UserId);

        await Mediator.Send(query);
        return NoContent();
    }
}