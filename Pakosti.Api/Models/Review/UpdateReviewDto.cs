using AutoMapper;
using Pakosti.Application.Common.Mappings;
using Pakosti.Application.Cqrs.Reviews.Commands.UpdateReview;

namespace Pakosti.Models.Review;

public class UpdateReviewDto : IMapWith<UpdateReviewCommand>
{
    public Guid Id { get; set; }
    public string? Header { get; set; }
    public string? Body { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateReviewCommand, UpdateReviewDto>()
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