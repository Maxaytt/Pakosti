using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Cqrs.Products.Queries.GetProduct;

public class GetProductQueryHandler :
    IRequestHandler<GetProductQuery, ProductVm>
{
    private readonly IPakostiDbContext _context;
    private readonly IMapper _mapper;


    public GetProductQueryHandler(IPakostiDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductVm> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);
        
        if (entity == null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == entity.CategoryId, cancellationToken);
        
        var vm = _mapper.Map<ProductVm>(entity);
        if (category != null)
        {
            vm.CategoryName = category.Name;
        }
        
        return vm;
    }
}