using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Mappings;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Reviews.Queries;

public static class GetReviewList
{
    public sealed record Query() : IRequest<Response>;

    public sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly IPakostiDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IPakostiDbContext context, IMapper mapper) =>
            (_context, _mapper) = (context, mapper);
        
        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        { 
            var reviews = await _context.Reviews
                .ProjectTo<LookupDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new Response(reviews);
        }
    }

    public sealed record Response(IList<LookupDto> Reviews);

    public sealed record LookupDto(Guid Id, Guid ProductId, string Header)
        : IMapWith<Review>
    {
        public void Mapping(Profile profile) =>
            profile.CreateMap<Review, LookupDto>();
    }
}