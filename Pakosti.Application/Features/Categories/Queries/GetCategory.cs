using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Common.Mappings;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Categories.Queries;

public static class GetCategory
{
    public sealed record Query(Guid Id) : IRequest<Response>;

    public sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly IPakostiDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IPakostiDbContext context, IMapper mapper) =>
            (_context, _mapper) = (context, mapper);
        
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (category == null) throw new NotFoundException(nameof(Category), request.Id);

            return _mapper.Map<Response>(category);
        }
    }

    public sealed record Response(Guid Id, Guid? ParentCategoryId, string Name) : IMapWith<Category>
    {
        public void Mapping(Profile profile) => 
            profile.CreateMap<Category, Response>();
    }
}