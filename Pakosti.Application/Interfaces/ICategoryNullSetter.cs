using Pakosti.Domain.Entities;

namespace Pakosti.Application.Interfaces;

public interface ICategoryNullSetter
{
    public Task SetNullCategoryChildes(Category entity);
}