using MediatR;

namespace Pakosti.Application.Features.Products.Queries.GetProduct;

public class GetProductQuery : IRequest<ProductVm>
{
    public Guid Id { get; set; }
}