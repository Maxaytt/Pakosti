using AutoMapper;
using Pakosti.Application.Common.Mappings;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Cqrs.Categories.Queries.GetCategory;

public class CategoryVm : IMapWith<Category>
{
    public Guid Id { get; set; }
    public Guid ParentCategoryId { get; set; }
    public string Name { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Category, CategoryVm>()
            .ForMember(c => c.Id,
                opt => opt
                    .MapFrom(c => c.Id))
            .ForMember(c => c.ParentCategoryId,
                opt => opt
                    .MapFrom(c => c.ParentCategoryId))
            .ForMember(c => c.Name,
                opt => opt
                    .MapFrom(c => c.Name));
    }
}