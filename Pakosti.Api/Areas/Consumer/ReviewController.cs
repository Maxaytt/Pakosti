using Mapster;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Api.BaseControllers;
using Pakosti.Application.Features.Consumer.Reviews.Commands;

namespace Pakosti.Api.Areas.Consumer;

public class ReviewController : ConsumerBaseController
{
    [HttpPost]
    public async Task<ActionResult> Crete([FromBody] CreateReview.Dto createReviewDto,
        CancellationToken cancellationToken)
    {
        var query = createReviewDto.Adapt<CreateReview.Command>()
            with { UserId = UserId };
        var response = await Mediator.Send(query, cancellationToken);
        return Created($"/api/review/{response.Id}", response);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateReview.Dto updateReviewDto,
        CancellationToken cancellationToken)
    {
        var query = updateReviewDto.Adapt<UpdateReview.Command>()
            with { UserId = UserId };
        await Mediator.Send(query, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id,
        CancellationToken cancellationToken)
    {
        var query = new DeleteReview.Command(id, UserId);

        await Mediator.Send(query, cancellationToken);
        return NoContent();
    }
}