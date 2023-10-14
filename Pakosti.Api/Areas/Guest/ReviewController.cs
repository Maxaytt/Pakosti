using Microsoft.AspNetCore.Mvc;
using Pakosti.Api.BaseControllers;
using Pakosti.Application.Features.Guest.Reviews.Queries;

namespace Pakosti.Api.Areas.Guest;

public class ReviewController : GuestBaseController
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> Get(Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetReview.Query(id);

        var vm = await Mediator.Send(query, cancellationToken);
        return Ok(vm);
    }
    
    [HttpGet]
    public async Task<ActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetReviewList.Query();

        var vm = await Mediator.Send(query, cancellationToken);
        return Ok(vm);
    }
}