namespace Pakosti.Application.Features.Products.Queries.GetProductList;

public class ProductListVm
{
    public IList<ProductLookupDto> Products { get; set; } = null!;
}