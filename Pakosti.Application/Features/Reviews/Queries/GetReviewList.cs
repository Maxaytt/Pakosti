using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Interfaces;

namespace Pakosti.Application.Features.Reviews.Queries;

public static class GetReviewList
{
    public sealed record Query() : IRequest<Response>;

    public sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context) =>
            _context = context;
        
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        { 
            var reviews = await _context.Reviews
                .ProjectToType<LookupDto>()
                .ToListAsync(cancellationToken);

            return new Response(reviews);
        }
    }

    public sealed record Response(IList<LookupDto> Reviews);

    public sealed record LookupDto(Guid Id, Guid ProductId, string Header);
}