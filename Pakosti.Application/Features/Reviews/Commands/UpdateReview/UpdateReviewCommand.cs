using MediatR;

namespace Pakosti.Application.Features.Reviews.Commands.UpdateReview;

public class UpdateReviewCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? Header { get; set; }
    public string? Body { get; set; }
}