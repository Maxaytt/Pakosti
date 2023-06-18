using MediatR;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Cqrs.Categories.Commands.CreateCategory;

public class CreateCategoryCommand : IRequest<Guid>
{
    public Guid? ParentCategoryId { get; set; }
    public string Name { get; set; } = null!;
}