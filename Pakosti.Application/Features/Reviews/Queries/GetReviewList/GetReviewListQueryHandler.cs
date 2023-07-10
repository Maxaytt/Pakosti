using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Interfaces;

namespace Pakosti.Application.Features.Reviews.Queries.GetReviewList;

public class GetReviewListQueryHandler
    : IRequestHandler<GetReviewListQuery, ReviewListVm>
{
    private readonly IPakostiDbContext _context;
    private readonly IMapper _mapper;

    public GetReviewListQueryHandler(IPakostiDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ReviewListVm> Handle(GetReviewListQuery request, CancellationToken cancellationToken)
    {
        var reviews = await _context.Reviews
            .ProjectTo<ReviewLookupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new ReviewListVm { Reviews = reviews };
    }
}