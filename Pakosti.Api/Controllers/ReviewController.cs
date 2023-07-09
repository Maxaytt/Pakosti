using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pakosti.Application.Cqrs.Categories.Queries.GetCategory;
using Pakosti.Application.Cqrs.Reviews.Commands.CreateReview;
using Pakosti.Application.Cqrs.Reviews.Commands.DeleteReview;
using Pakosti.Application.Cqrs.Reviews.Commands.UpdateReview;
using Pakosti.Application.Cqrs.Reviews.Queries.GetReview;
using Pakosti.Application.Cqrs.Reviews.Queries.GetReviewList;
using Pakosti.Api.Models.Review;

namespace Pakosti.Api.Controllers;

public class ReviewController : BaseController
{
    private readonly IMapper _mapper;

    public ReviewController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<ReviewListVm>> GetAll()
    {
        var query = new GetReviewListQuery();

        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReviewVm>> Get(Guid id)
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
    public async Task<ActionResult<Guid>> Crete([FromBody] CreateReviewDto createReviewDto)
    {
        var query = _mapper.Map<CreateReviewCommand>(createReviewDto);
        query.UserId = UserId;
        var id = await Mediator.Send(query);
        return Ok(id);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> Update([FromBody] UpdateReviewDto updateReviewDto)
    {
        var query = _mapper.Map<UpdateReviewCommand>(updateReviewDto);
        query.UserId = UserId;
        await Mediator.Send(query);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> Delete(Guid id)
    {
        var query = new DeleteReviewCommand
        {
            Id = id,
            UserId = UserId
        };

        await Mediator.Send(query);
        return NoContent();
    }
}