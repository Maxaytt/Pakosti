
namespace Pakosti.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string Name { get; set; } = null!;
}