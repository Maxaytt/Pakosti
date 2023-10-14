using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

// ReSharper disable UnusedType.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable NotAccessedPositionalProperty.Global

namespace Pakosti.Application.Features.Guest.Products.Queries;

public static class GetProduct
{
    public sealed record Query(Guid Id) : IRequest<Response>;

    public sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context) =>
            _context = context;

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);
        
            if (entity == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == entity.CategoryId, cancellationToken);
        
            var response = entity.Adapt<Response>();
            if (category != null)
            {
                response = response with { CategoryName = category.Name };
            }
        
            return response;
        }
    }

    public sealed record Response(
        Guid Id, Guid? CategoryId,
        string? CategoryName, string Name, string Description,
        DateTimeOffset CreationDate, DateTimeOffset? EditionDate);
}