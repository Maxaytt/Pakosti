using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Interfaces;

namespace Pakosti.Application.Features.Products.Queries.GetProductList;

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
        var products = _context.Products;
        var projected = products.ProjectTo<ProductLookupDto>(_mapper.ConfigurationProvider);
        var entities = await projected.ToListAsync(cancellationToken);

        return new ProductListVm { Products = entities };
    }
}