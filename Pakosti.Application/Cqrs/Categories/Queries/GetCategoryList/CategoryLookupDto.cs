using AutoMapper;
using Pakosti.Application.Common.Mappings;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Cqrs.Categories.Queries.GetCategoryList;

public class CategoryLookupDto : IMapWith<Category>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Category, CategoryLookupDto>()
            .ForMember(c => c.Id,
                opt => opt
                    .MapFrom(dto => dto.Id))
            .ForMember(c => c.Name,
                opt => opt
                    .MapFrom(dto => dto.Name));
    }
}