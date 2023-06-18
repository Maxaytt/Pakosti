using MediatR;

namespace Pakosti.Application.Cqrs.Products.Commands.CreateProduct;

public class CreateProductCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}