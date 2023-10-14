using System.Collections.ObjectModel;

namespace Pakosti.Domain.Entities;

public class Cart
{
    public Guid UserId { get; set; } 
    public ICollection<CartItem> CartItems { get; set; } = new Collection<CartItem>();
}