using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Categories.Queries.GetCategory;

public class GetCategoryQueryHandler
    : IRequestHandler<GetCategoryQuery, CategoryVm>
{
    private readonly IPakostiDbContext _context;
    private readonly IMapper _mapper;

    public GetCategoryQueryHandler(IPakostiDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CategoryVm> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category == null) throw new NotFoundException(nameof(Category), request.Id);

        return _mapper.Map<CategoryVm>(category);
    }
}