using AutoMapper;
using Pakosti.Application.Common.Mappings;
using Pakosti.Application.Cqrs.Reviews.Commands.CreateReview;

namespace Pakosti.Api.Models.Review;

public class CreateReviewDto : IMapWith<CreateReviewCommand>
{
    public Guid ProductId { get; set; }
    public string Header { get; set; } = null!;
    public string Body { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateReviewCommand, CreateReviewDto>()
            .ForMember(dto => dto.ProductId,
                opt => opt
                    .MapFrom(c => c.ProductId))
            .ForMember(dto => dto.Header,
                opt => opt
                    .MapFrom(c => c.Header))
            .ForMember(dto => dto.Body,
                opt => opt
                    .MapFrom(c => c.Body));
    }
}