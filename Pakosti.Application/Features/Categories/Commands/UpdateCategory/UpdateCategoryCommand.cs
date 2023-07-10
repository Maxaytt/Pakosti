using MediatR;

namespace Pakosti.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string? Name { get; set; }
}