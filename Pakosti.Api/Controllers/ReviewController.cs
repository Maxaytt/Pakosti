using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Api.Models.Review;
using Pakosti.Application.Features.Reviews.Commands;
using Pakosti.Application.Features.Reviews.Queries;

namespace Pakosti.Api.Controllers;

public class ReviewController : BaseController
{
    private readonly IMapper _mapper;

    public ReviewController(IMapper mapper)
    {
        _mapper = mapper;
    }

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
    public async Task<ActionResult<Guid>> Crete([FromBody] CreateReviewDto createReviewDto)
    {
        var query = _mapper.Map<CreateReview.Command>(createReviewDto);
        query = query with { UserId = UserId };
        var id = await Mediator.Send(query);
        return Ok(id);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> Update([FromBody] UpdateReviewDto updateReviewDto)
    {
        var query = _mapper.Map<UpdateReview.Command>(updateReviewDto);
        query = query with { UserId = UserId };
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