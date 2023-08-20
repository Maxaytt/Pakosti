using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pakosti.Application.Exceptions;
using Pakosti.Application.Extensions.ValidationExtensions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Categories.Commands;

public static class UpdateCategory
{
    public sealed record Command(Guid Id, Guid? ParentCategoryId, string? Name) : IRequest;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator(IConfiguration configuration) => 
            RuleFor(c => c.Name).CategoryName(configuration);
    }
    
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

            if (request.ParentCategoryId != null)
            {
                var parentCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == request.ParentCategoryId, cancellationToken);
            
                if (parentCategory == null) throw new NotFoundException(nameof(Category), request.ParentCategoryId);
            }
        
            category.ParentCategoryId = request.ParentCategoryId;

            if (request.Name != null)
            {
                category.Name = request.Name;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}