using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

// ReSharper disable UnusedType.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable NotAccessedPositionalProperty.Global

namespace Pakosti.Application.Features.Guest.Reviews.Queries;

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
        DateTimeOffset CreationDate, DateTimeOffset? EditionDate);
}