using Pakosti.Domain.Entities;
using Pakosti.Infrastructure.Persistence;

namespace Pakosti.Tests.Common.CqrsFactories;

public class ReviewContextFactory : ContextFactory
{
    public readonly Guid ReviewIdForDelete = Guid.NewGuid();
    public readonly Guid ReviewIdForUpdate = Guid.NewGuid();
    private static Guid _productId = Guid.NewGuid();
    public Guid ProductId = _productId;
    

    public readonly Review ReviewForGet = new()
    {
        Id = Guid.NewGuid(),
        ProductId = Guid.NewGuid(),
        UserId = UserAId,
        Header = "test review",
        Body = "bla bla",
        CreationDate = DateTime.Now,
        EditionDate = null
    };

    public readonly List<Review> Reviews = new()
    {
        new Review
        {
            Id = Guid.NewGuid(),
            ProductId = _productId,
            UserId = UserAId,
            Header = "test review 1",
            Body = "bla bla",
            CreationDate = DateTime.Now,
            EditionDate = null
        },
        new Review
        {
            Id = Guid.NewGuid(),
            ProductId = _productId,
            UserId = UserAId,
            Header = "test review 2",
            Body = "bla bla",
            CreationDate = DateTime.Now,
            EditionDate = null
        },
        new Review
        {
            Id = Guid.NewGuid(),
            ProductId = _productId,
            UserId = UserAId,
            Header = "test review 3",
            Body = "bla bla",
            CreationDate = DateTime.Now,
            EditionDate = null
        }
    };

    public async Task SetUpForCreate(PakostiDbContext context)
    {
        await context.Products.AddAsync(new Product
        {
            Id = ProductId,
            CategoryId = null,
            Name = "test product",
            Description = "bla bla",
            CreationDate = DateTime.Now,
            EditionDate = null,
            UserId = UserAId
        }, CancellationToken.None);
        await context.SaveChangesAsync(CancellationToken.None);
    }

    public async Task SetUpForUpdate(PakostiDbContext context)
    {
        await SetUpForCreate(context);
        await context.Reviews.AddAsync(new Review
        {
            Id = ReviewIdForUpdate,
            ProductId = ProductId,
            Body = "bad product",
            Header = "test review",
            CreationDate = DateTime.Now,
            EditionDate = null,
            UserId = UserAId
        }, CancellationToken.None);
        await context.SaveChangesAsync(CancellationToken.None);
    }

    public async Task SetUpForDelete(PakostiDbContext context)
    {
        await context.Reviews.AddAsync(new Review
        {
            Id = ReviewIdForDelete,
            ProductId = ProductId,
            Body = "bad product",
            Header = "test review",
            CreationDate = DateTime.Now,
            EditionDate = null,
            UserId = UserAId
        }, CancellationToken.None);
        await context.SaveChangesAsync(CancellationToken.None);
    }

    public async Task SetUpForGetting(PakostiDbContext context)
    {
        await context.Reviews.AddAsync(ReviewForGet, CancellationToken.None);
        await context.SaveChangesAsync(CancellationToken.None);
    }

    public async Task SetUpForGettingList(PakostiDbContext context)
    {
        await context.Reviews.AddRangeAsync(Reviews, CancellationToken.None);
        await context.SaveChangesAsync(CancellationToken.None);
    }
}