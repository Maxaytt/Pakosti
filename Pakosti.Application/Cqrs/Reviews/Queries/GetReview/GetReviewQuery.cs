using MediatR;

namespace Pakosti.Application.Cqrs.Reviews.Queries.GetReview;

public class GetReviewQuery : IRequest<ReviewVm>
{
    public Guid Id { get; set; }
}