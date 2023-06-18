using MediatR;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Cqrs.Products.Commands.UpdateProduct;

public class UpdateProductCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}