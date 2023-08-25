using Mapster;
using Pakosti.Application.Features.Carts.Queries;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Mappings;

public static class MapsterConfig
{
    public static void Setup()
    {
        TypeAdapterConfig<CartItem, GetItemList.LookupDto>
            .NewConfig()
            .Map(dest => dest.Product, src => new GetItemList.ProductDto(
                    src.Product.Name, src.Product.Price.Cost, src.Product.Price.CurrencyName));
        
        TypeAdapterConfig<CartItem, GetItem.Response>
            .NewConfig()
            .Map(dest => dest.Product, src => new GetItem.ProductDto(
                src.Product.Name, src.Product.Price.Cost, src.Product.Price.CurrencyName));
    }
}