using MediatR;

namespace Pakosti.Application.Features.Categories.Queries.GetCategory;

public class GetCategoryQuery : IRequest<CategoryVm>
{
    public Guid Id { get; set; }
}