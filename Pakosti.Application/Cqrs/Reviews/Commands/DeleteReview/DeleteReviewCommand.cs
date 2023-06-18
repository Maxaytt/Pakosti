using System.Data.Common;
using MediatR;

namespace Pakosti.Application.Cqrs.Reviews.Commands.DeleteReview;

public class DeleteReviewCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}