using MediatR;

namespace Pakosti.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public string Header { get; set; } = null!;
    public string Body { get; set; } = null!;
}