using MediatR;
using Pakosti.Domain.Entities;

namespace Pakosti.Application.Cqrs.Categories.Queries.GetCategory;

public class GetCategoryQuery : IRequest<CategoryVm>
{
    public Guid Id { get; set; }
}