using AutoMapper;
using Pakosti.Application.Common.Mappings;
using Pakosti.Application.Features.Categories.Commands;

namespace Pakosti.Api.Models.Category;

public class CreateCategoryDto : IMapWith<CreateCategory.Command>
{
    public Guid? ParentCategoryId { get; set; }
    public string Name { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateCategory.Command, CreateCategoryDto>()
            .ForMember(dto => dto.ParentCategoryId,
                opt => opt
                    .MapFrom(c => c.ParentCategoryId))
            .ForMember(dto => dto.Name,
                opt => opt
                    .MapFrom(c => c.Name));
    }
}