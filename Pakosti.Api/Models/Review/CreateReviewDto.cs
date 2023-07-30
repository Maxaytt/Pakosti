using AutoMapper;
using Pakosti.Application.Common.Mappings;
using Pakosti.Application.Features.Reviews.Commands;

namespace Pakosti.Api.Models.Review;

public class CreateReviewDto : IMapWith<CreateReview.Command>
{
    public Guid ProductId { get; set; }
    public string Header { get; set; } = null!;
    public string Body { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateReview.Command, CreateReviewDto>()
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