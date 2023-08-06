using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Interfaces;

namespace Pakosti.Application.Features.Categories.Queries;

public static class GetCategoryList
{
    public sealed record Query : IRequest<Response>;

    public sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context) =>
            _context = context;
        
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var categories = await _context.Categories
                .ProjectToType<LookupDto>()
                .ToListAsync(cancellationToken);

            return new Response(categories);
        }
    }

    public sealed record Response(IList<LookupDto> Categories);

    public sealed record LookupDto(Guid Id, string Name);
}