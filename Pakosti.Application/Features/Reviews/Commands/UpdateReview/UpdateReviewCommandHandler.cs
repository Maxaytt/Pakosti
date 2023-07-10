using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Reviews.Commands.UpdateReview;

public class UpdateReviewCommandHandler
    : IRequestHandler<UpdateReviewCommand>
{
    private readonly IPakostiDbContext _context;

    public UpdateReviewCommandHandler(IPakostiDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _context.Reviews
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (review == null || review.UserId != request.UserId)
        {
            throw new NotFoundException(nameof(Review), request.Id);
        }
        
        review.EditionDate = DateTime.Now;
        if (request.Header != null)
        {
            review.Header = request.Header;
        }
        if (request.Body != null)
        {
            review.Body = request.Body;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}