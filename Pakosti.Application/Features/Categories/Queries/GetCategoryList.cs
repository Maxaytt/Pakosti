using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Mappings;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Categories.Queries;

public static class GetCategoryList
{
    public sealed record Query : IRequest<Response>;

    public sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly IPakostiDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IPakostiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var categories = await _context.Categories
                .ProjectTo<LookupDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new Response(categories);
        }
    }

    public sealed record Response(IList<LookupDto> Categories);

    public sealed record LookupDto(Guid Id, string Name) : IMapWith<Category>
    {
        public void Mapping(Profile profile) =>
            profile.CreateMap<Category, LookupDto>();
    }
}