using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Cqrs.Reviews.Commands.DeleteReview;

public class DeleteReviewCommandHandler
    : IRequestHandler<DeleteReviewCommand>
{
    private readonly IPakostiDbContext _context;

    public DeleteReviewCommandHandler(IPakostiDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _context.Reviews
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (review == null || review.UserId != request.UserId)
        {
            throw new NotFoundException(nameof(Review), request.Id);
        }

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync(cancellationToken);
    }
}