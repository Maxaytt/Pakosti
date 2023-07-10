namespace Pakosti.Application.Features.Categories.Queries.GetCategoryList;

public class CategoryListVm
{
    public IList<CategoryLookupDto> Categories { get; set; } = null!;
}