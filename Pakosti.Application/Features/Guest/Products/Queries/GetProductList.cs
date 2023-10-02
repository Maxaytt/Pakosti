using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Interfaces;

// ReSharper disable UnusedType.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable NotAccessedPositionalProperty.Global

namespace Pakosti.Application.Features.Guest.Products.Queries;

public static class GetProductList
{
    public sealed record Query() : IRequest<Response>;

    public sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context) =>
            _context = context;
        
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var products = _context.Products;
            var projected = products.ProjectToType<LookupDto>();
            var entities = await projected.ToListAsync(cancellationToken);

            return new Response(entities);
        }
    }

    public sealed record Response(IList<LookupDto> Products);

    public sealed record LookupDto(Guid Id, string Name);
}