using System.Collections;

namespace Pakosti.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public List<Guid>? ProductsId { get; set; }
    public List<Category>? SubCategories { get; set; }
}