using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Reviews.Queries;

public static class GetReview
{
    public sealed record Query(Guid Id) : IRequest<Response>;

    public sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context) =>
            _context = context;

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (review == null)
            {
                throw new NotFoundException(nameof(Review), request.Id);
            }

            return review.Adapt<Response>();
        }
    }

    public sealed record Response(Guid Id, Guid ProductId,
        string Header, string Body,
        DateTime CreationDate, DateTime? EditionDate);
}