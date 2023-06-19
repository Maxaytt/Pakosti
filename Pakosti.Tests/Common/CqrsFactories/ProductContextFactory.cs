using Pakosti.Domain.Entities;
using Pakosti.Infrastructure.Persistence;

namespace Pakosti.Tests.Common.CqrsFactories;

public class ProductContextFactory : ContextFactory
{
    public Guid ProductIdForDelete = Guid.NewGuid();
    public Guid ProductIdForUpdate = Guid.NewGuid();
    public Guid CategoryId = Guid.NewGuid();

    public async Task SetUpForCreation(PakostiDbContext context)
    {
        context.Categories.Add(new Category
        {
            Id = CategoryId,
            ParentCategoryId = null,
            Name = "Test Category"
        });
        await context.SaveChangesAsync(CancellationToken.None);
    }

    public async Task SetUpForUpdate(PakostiDbContext context)
    {
        await SetUpForCreation(context);
        await context.Products.AddAsync(new Product
        {
            Id = ProductIdForUpdate,
            CategoryId = CategoryId,
            CreationDate = DateTime.Now,
            EditionDate = null,
            Name = "Test product",
            Description = "asdffa",
            UserId = UserAId
        });
        await context.SaveChangesAsync(CancellationToken.None);
    }

    public async Task SetUpForDelete(PakostiDbContext context)
    {
        await context.Products.AddAsync(new Product
        {
            Id = ProductIdForDelete,
            CategoryId = CategoryId,
            CreationDate = DateTime.Now,
            EditionDate = null,
            Name = "Test product",
            Description = "description",
            UserId = UserAId
        });

        await context.Reviews.AddRangeAsync(
        new Review
        {
            Id = Guid.NewGuid(),
            ProductId = ProductIdForDelete,
            UserId = UserAId,
            Header = "test header1",
            Body = "test body1",
            CreationDate = DateTime.Today,
            EditionDate = null
        },
        new Review
        {
            Id = Guid.NewGuid(),
            ProductId = ProductIdForDelete,
            UserId = UserAId,
            Header = "test header2",
            Body = "test body2",
            CreationDate = DateTime.Today,
            EditionDate = null
        });
        await context.SaveChangesAsync(CancellationToken.None);
    }
}