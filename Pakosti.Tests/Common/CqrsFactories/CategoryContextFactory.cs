using Pakosti.Domain.Entities;
using Pakosti.Infrastructure.Persistence;

namespace Pakosti.Tests.Common.CqrsFactories;

public class CategoryContextFactory : ContextFactory
{
    public readonly Guid CategoryIdForDelete = Guid.NewGuid();
    public readonly Guid CategoryIdForUpdate = Guid.NewGuid();
    public readonly Guid ParentCategoryId = Guid.NewGuid();
    public readonly Guid ChildCategoryId = Guid.NewGuid();

    public readonly Category CategoryForGet = new()
    {
        Id = Guid.NewGuid(),
        ParentCategoryId = null,
        Name = "test category"
    };

    public readonly List<Category> Categories = new List<Category>
    {
        new()
        {
            Id = Guid.NewGuid(),
            Name = "test category 1"
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = "test category 2"
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = "test category 3"
        }
    };


    public async Task SetUpForCreation(PakostiDbContext context)
    {
        context.Categories.Add(new Category
        {
            Id = ParentCategoryId,
            ParentCategoryId = null,
            Name = "Test Parent Category"
        });
        await context.SaveChangesAsync(CancellationToken.None);
    }

    public async Task SetUpForUpdate(PakostiDbContext context)
    {
        await SetUpForCreation(context);
        
        await context.Categories.AddAsync(new Category
        {
            Id = CategoryIdForUpdate,
            ParentCategoryId = ParentCategoryId,
            Name = "Test Category"
        });
        await context.SaveChangesAsync(CancellationToken.None);
    }

    public async Task SetUpForDelete(PakostiDbContext context)
    {
        await context.Categories.AddRangeAsync(
            new Category
            {
                Id = CategoryIdForDelete,
                ParentCategoryId = null,
                Name = "Category to delete"
            },
            new Category
            {
                Id = ChildCategoryId,
                ParentCategoryId = CategoryIdForDelete,
                Name = "test child"
            });

        await context.SaveChangesAsync(CancellationToken.None);
    }

    public async Task SetUpForGetting(PakostiDbContext context)
    {
        await context.AddAsync(CategoryForGet, CancellationToken.None);
        await context.SaveChangesAsync(CancellationToken.None);
    }

    public async Task SetUpForGettingList(PakostiDbContext context)
    {
        await context.AddRangeAsync(Categories, CancellationToken.None);
        await context.SaveChangesAsync(CancellationToken.None);
    }
}