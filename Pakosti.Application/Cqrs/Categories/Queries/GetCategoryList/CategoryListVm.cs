using Pakosti.Domain.Entities;

namespace Pakosti.Application.Cqrs.Categories.Queries.GetCategoryList;

public class CategoryListVm
{
    public IList<CategoryLookupDto> Categories { get; set; } = null!;
}