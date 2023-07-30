using AutoMapper;
using Pakosti.Application.Common.Mappings;
using Pakosti.Application.Features.Reviews.Commands;

namespace Pakosti.Api.Models.Review;

public class UpdateReviewDto : IMapWith<UpdateReview.Command>
{
    public Guid Id { get; set; }
    public string? Header { get; set; }
    public string? Body { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateReview.Command, UpdateReviewDto>()
            .ForMember(dto => dto.Id,
                opt => opt
                    .MapFrom(r => r.Id))
            .ForMember(dto => dto.Header,
                opt => opt
                    .MapFrom(r => r.Header))
            .ForMember(dto => dto.Body,
                opt => opt
                    .MapFrom(r => r.Body));
    }
}