using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable once UnusedType.Global

namespace Pakosti.Application.Features.Administrator.Categories.Queries;

public static class GetCategory
{
    public sealed record Query(Guid Id) : IRequest<Response>;

    public sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context) =>
            _context = context;
        
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (category == null) throw new NotFoundException(nameof(Category), request.Id);

            return category.Adapt<Response>();
        }
    }

    public sealed record Response(Guid Id, Guid? ParentCategoryId, string Name);
}