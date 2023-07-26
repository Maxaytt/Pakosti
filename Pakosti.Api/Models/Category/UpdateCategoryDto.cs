using AutoMapper;
using Pakosti.Application.Common.Mappings;
using Pakosti.Application.Features.Categories.Commands;

namespace Pakosti.Api.Models.Category;

public class UpdateCategoryDto : IMapWith<UpdateCategory.Command>
{
    public Guid Id { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string? Name { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateCategory.Command, UpdateCategoryDto>()
            .ForMember(dto => dto.Id,
                opt => opt
                    .MapFrom(u => u.Id))
            .ForMember(dto => dto.ParentCategoryId,
                opt => opt
                    .MapFrom(u => u.ParentCategoryId))
            .ForMember(dto => dto.Name,
                opt => opt
                    .MapFrom(u => u.Name));
    }
}