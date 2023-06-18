using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Interfaces;

namespace Pakosti.Application.Cqrs.Products.Queries.GetProductList;

public class GetProductListQueryHandler 
    : IRequestHandler<GetProductListQuery, ProductListVm>
{
    private readonly IPakostiDbContext _context;
    private readonly IMapper _mapper;

    public GetProductListQueryHandler(IPakostiDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductListVm> Handle(GetProductListQuery request, CancellationToken cancellationToken)
    {
        var entities = await _context.Products
            .ProjectTo<ProductLookupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new ProductListVm { Products = entities };
    }
}