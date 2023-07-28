using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Mappings;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Products.Queries;

public static class GetProductList
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
            var products = _context.Products;
            var projected = products.ProjectTo<LookupDto>(_mapper.ConfigurationProvider);
            var entities = await projected.ToListAsync(cancellationToken);

            return new Response(entities);
        }
    }

    public sealed record Response(IList<LookupDto> Products);

    public sealed record LookupDto(Guid Id, string Name) : IMapWith<Product>
    {
        public void Mapping(Profile profile) =>
            profile.CreateMap<Product, LookupDto>();
    };
}