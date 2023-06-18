using AutoMapper;
using Pakosti.Application.Common.Mappings;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Cqrs.Reviews.Queries.GetReviewList;

public class ReviewLookupDto : IMapWith<Review>
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string Header { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Review, ReviewLookupDto>()
            .ForMember(dto => dto.Id,
                opt => opt
                    .MapFrom(r => r.Id))
            .ForMember(dto => dto.ProductId,
                opt => opt
                    .MapFrom(r => r.ProductId))
            .ForMember(dto => dto.Header,
                opt => opt
                    .MapFrom(r => r.Header));
    }
}