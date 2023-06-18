using MediatR;

namespace Pakosti.Application.Cqrs.Products.Queries.GetProduct;

public class GetProductQuery : IRequest<ProductVm>
{
    public Guid Id { get; set; }
}