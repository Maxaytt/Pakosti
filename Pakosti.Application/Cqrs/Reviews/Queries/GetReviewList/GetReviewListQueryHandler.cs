using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Interfaces;

namespace Pakosti.Application.Cqrs.Reviews.Queries.GetReviewList;

public class GetReviewListQueryHandler
    : IRequestHandler<GetReviewListQuery, ReviewListVm>
{
    private readonly IPakostiDbContext _context;
    private readonly IMapper _mapper;

    public GetReviewListQueryHandler(IMapper mapper, IPakostiDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<ReviewListVm> Handle(GetReviewListQuery request, CancellationToken cancellationToken)
    {
        var reviews = await _context.Reviews
            .ProjectTo<ReviewLookupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new ReviewListVm { Reviews = reviews };
    }
}