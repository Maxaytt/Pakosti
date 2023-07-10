using MediatR;

namespace Pakosti.Application.Features.Reviews.Queries.GetReview;

public class GetReviewQuery : IRequest<ReviewVm>
{
    public Guid Id { get; set; }
}