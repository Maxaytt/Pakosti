using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Common.Mappings;
using Pakosti.Application.Interfaces;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Reviews.Queries;

public static class GetReview
{
    public sealed record Query(Guid Id) : IRequest<Response>;

    public sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly IPakostiDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IPakostiDbContext context, IMapper mapper) =>
            (_context, _mapper) = (context, mapper);

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (review == null)
            {
                throw new NotFoundException(nameof(Review), request.Id);
            }

            return _mapper.Map<Response>(review);
        }
    }

    public sealed record Response(Guid Id, Guid ProductId,
        string Header, string Body,
        DateTime CreationDate, DateTime? EditionDate) : IMapWith<Review>
    {
        public void Mapping(Profile profile) =>
            profile.CreateMap<Review, Response>();
    };
}