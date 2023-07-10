using MediatR;

namespace Pakosti.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommand : IRequest<Guid>
{
    public Guid? ParentCategoryId { get; set; }
    public string Name { get; set; } = null!;
}