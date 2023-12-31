using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pakosti.Application.Exceptions;
using Pakosti.Application.Extensions.ValidationExtensions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

// ReSharper disable UnusedType.Global

namespace Pakosti.Application.Features.Administrator.Categories.Commands;

public static class CreateCategory
{
    public sealed record Command(Guid? ParentCategoryId, string Name) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator(IConfiguration configuration) => RuleFor(c => c.Name)
            .CategoryName(configuration).NotNull().WithMessage("Name is required");
    }

    public sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly IPakostiDbContext _context;

        public Handler(IPakostiDbContext context)
        {
            _context = context;
        }
        
        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.ParentCategoryId != null)
            {
                var parentCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == request.ParentCategoryId, cancellationToken);
            
                if (parentCategory == null) throw new NotFoundException(nameof(Category), request.ParentCategoryId);
            }

            var product = new Category
            {
                Id = Guid.NewGuid(),
                ParentCategoryId = request.ParentCategoryId,
                Name = request.Name
            };

            await _context.Categories.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new Response(product.Id);
        }
    }

    public sealed record Response(Guid Id);
}