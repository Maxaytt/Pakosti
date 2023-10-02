using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Administrator.Categories.Commands;

public static class DeleteCategory
{
    public sealed record Command(Guid Id) : IRequest;
    
    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context)
        {
            _context = context;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (category == null) throw new NotFoundException(nameof(Category), request.Id);

            _context.Categories.Remove(category);
            await _context.SetNullCategoryChildes(category);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}