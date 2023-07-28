using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Common.Mappings;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Products.Queries;

public static class GetProduct
{
    public sealed record Command(Guid Id) : IRequest<Response>;

    public sealed class Handler : IRequestHandler<Command, Response>
    {
        private readonly IPakostiDbContext _context;
        private readonly IMapper _mapper;
        
        public Handler(IPakostiDbContext context, IMapper mapper) =>
            (_context, _mapper) = (context, mapper);

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);
        
            if (entity == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == entity.CategoryId, cancellationToken);
        
            var response = _mapper.Map<Response>(entity);
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
        DateTime CreationDate, DateTime? EditionDate) : IMapWith<Product>
    {
        public void Mapping(Profile profile) =>
            profile.CreateMap<Product, Response>();
        }
}