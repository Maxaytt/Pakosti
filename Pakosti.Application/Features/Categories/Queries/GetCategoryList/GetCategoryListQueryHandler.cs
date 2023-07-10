using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Interfaces;

namespace Pakosti.Application.Features.Categories.Queries.GetCategoryList;

public class GetCategoryListQueryHandler
    : IRequestHandler<GetCategoryListQuery, CategoryListVm>
{
    private readonly IPakostiDbContext _context;
    private readonly IMapper _mapper;

    public GetCategoryListQueryHandler(IPakostiDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CategoryListVm> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories
            .ProjectTo<CategoryLookupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new CategoryListVm { Categories = categories };
    }
}