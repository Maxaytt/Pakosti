using MediatR;

namespace Pakosti.Application.Cqrs.Products.Commands.DeleteProduct;

public class DeleteProductCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}