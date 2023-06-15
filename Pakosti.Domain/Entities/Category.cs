using System.Collections;

namespace Pakosti.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string Name { get; set; } = null!;
    public Category? ParentCategory { get; set; }
    
    public ICollection<Product>? Products { get; set; }
    public ICollection<Category>? SubCategories { get; set; }
}