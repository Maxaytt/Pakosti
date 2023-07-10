using AutoMapper;
using Pakosti.Application.Common.Mappings;
using Pakosti.Application.Features.Categories.Commands.CreateCategory;

namespace Pakosti.Api.Models.Category;

public class CreateCategoryDto : IMapWith<CreateCategoryCommand>
{
    public Guid? ParentCategoryId { get; set; }
    public string Name { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateCategoryCommand, CreateCategoryDto>()
            .ForMember(dto => dto.ParentCategoryId,
                opt => opt
                    .MapFrom(c => c.ParentCategoryId))
            .ForMember(dto => dto.Name,
                opt => opt
                    .MapFrom(c => c.Name));
    }
}