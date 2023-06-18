using MediatR;

namespace Pakosti.Application.Cqrs.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommand : IRequest
{
    public Guid Id { get; set; }
}