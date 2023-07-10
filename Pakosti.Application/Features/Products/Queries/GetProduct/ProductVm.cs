using AutoMapper;
using Pakosti.Application.Common.Mappings;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Features.Products.Queries.GetProduct;

public class ProductVm : IMapWith<Product>
{
    public Guid Id { get; set; }
    public Guid? CategoryId { get; set; }
    public string? CategoryName { get; set; } 
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime CreationDate { get; set; }
    public DateTime? EditionDate { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<Product, ProductVm>()
            .ForMember(pvm => pvm.Id,
                opt => opt.MapFrom(p => p.Id))
            .ForMember(pvm => pvm.CategoryId,
                opt => opt.MapFrom(p => p.CategoryId))
            .ForMember(pvm => pvm.Name,
                opt => opt.MapFrom(p => p.Name))
            .ForMember(pvm => pvm.Description,
                opt => opt.MapFrom(p => p.Description))
            .ForMember(pvm => pvm.CreationDate,
                opt => opt.MapFrom(p => p.CreationDate))
            .ForMember(pvm => pvm.EditionDate,
                opt => opt.MapFrom(p => p.EditionDate));
    }
}