using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Cqrs.Reviews.Queries.GetReview;

public class GetReviewQueryHandler
    : IRequestHandler<GetReviewQuery, ReviewVm>
{
    private readonly IPakostiDbContext _context;
    private readonly IMapper _mapper;

    public GetReviewQueryHandler(IPakostiDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ReviewVm> Handle(GetReviewQuery request, CancellationToken cancellationToken)
    {
        var review = await _context.Reviews
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (review == null)
        {
            throw new NotFoundException(nameof(Review), request.Id);
        }

        return _mapper.Map<ReviewVm>(review);
    }
}