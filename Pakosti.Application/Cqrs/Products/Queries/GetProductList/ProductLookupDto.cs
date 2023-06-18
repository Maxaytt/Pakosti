using AutoMapper;
using Pakosti.Application.Common.Mappings;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Cqrs.Products.Queries.GetProductList;

public class ProductLookupDto : IMapWith<Product>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Product, ProductLookupDto>()
            .ForMember(p => p.Id,
                opt => opt.MapFrom(dto => dto.Id))
            .ForMember(p => p.Name,
                opt => opt.MapFrom(dto => dto.Name));
    }
}