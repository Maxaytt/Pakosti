using MediatR;
using Pakosti.Domain.Entities;
using Pakosti.Application.Interfaces;

namespace Pakosti.Application.Cqrs.Reviews.Commands.CreateReview;

public class CreateReviewCommandHandler
    : IRequestHandler<CreateReviewCommand, Guid>
{
    private readonly IPakostiDbContext _context;

    public CreateReviewCommandHandler(IPakostiDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var review = new Review
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            ProductId = request.ProductId,
            Header = request.Header,
            Body = request.Body,
            CreationDate = DateTime.Now,
            EditionDate = null
        };

        await _context.Reviews.AddAsync(review, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return review.Id;
    }
}