using AutoMapper;
using Pakosti.Application.Common.Mappings;
using Pakosti.Application.Features.Products.Commands;

namespace Pakosti.Api.Models.Product;

public class UpdateProductDto : IMapWith<UpdateProduct.Command>
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string? Name { get; set; } = null!;
    public string? Description { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateProductDto, UpdateProduct.Command>()
            .ForMember(u => u.Id,
                opt =>
                    opt.MapFrom(u => u.Id))
            .ForMember(u => u.CategoryId,
                opt =>
                    opt.MapFrom(u => u.CategoryId))
            .ForMember(u => u.Name,
                opt =>
                    opt.MapFrom(u => u.Name))
            .ForMember(u => u.Description,
                opt =>
                    opt.MapFrom(u => u.Description));
    }
}