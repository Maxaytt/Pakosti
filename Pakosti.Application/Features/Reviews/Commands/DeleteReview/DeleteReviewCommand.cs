using MediatR;

namespace Pakosti.Application.Features.Reviews.Commands.DeleteReview;

public class DeleteReviewCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}