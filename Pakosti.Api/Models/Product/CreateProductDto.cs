using AutoMapper;
using Pakosti.Application.Common.Mappings;
using Pakosti.Application.Cqrs.Products.Commands.CreateProduct;

namespace Pakosti.Models.Product;

public class CreateProductDto : IMapWith<CreateProductCommand>
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateProductDto, CreateProductCommand>()
            .ForMember(c => c.CategoryId,
                opt =>
                    opt.MapFrom(c => c.CategoryId))
            .ForMember(c => c.Name,
                opt =>
                    opt.MapFrom(c => c.Name))
            .ForMember(c => c.Description,
                opt =>
                    opt.MapFrom(c => c.Description));
    }
}