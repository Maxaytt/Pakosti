using MediatR;

namespace Pakosti.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}