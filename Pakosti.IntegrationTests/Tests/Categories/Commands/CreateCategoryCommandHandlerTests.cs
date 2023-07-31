using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Features.Categories.Commands;
using Pakosti.Domain.Entities;
using Pakosti.IntegrationTests.Common;
using Pakosti.IntegrationTests.Common.CqrsFactories;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.Categories.Commands;

public class CreateCategoryCommandHandlerTests : TestCommandBase
{
    private readonly CategoryContextFactory _contextFactory = new ();

    [Fact]
    public async Task Create_WithoutParent_CategoryAddedToDatabase()
    {
        // Arrange
        await _contextFactory.SetUpForCreation(Context);

        var handler = new CreateCategory.Handler(Context);
        var query = new CreateCategory.Command(null, "Test category");
        
        // Act
        var id = await handler.Handle(query, CancellationToken.None);
        
        // Assert
        var category = await Context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        
        category.ShouldNotBeNull();
        category.ParentCategoryId.ShouldBeNull();
        category.Name.ShouldBe(query.Name);
    }
    
    [Fact]
    public async Task Create_WithParent_CategoryAddedToDatabase()
    {
        // Arrange
        await _contextFactory.SetUpForCreation(Context);

        var handler = new CreateCategory.Handler(Context);
        var query = new CreateCategory.Command(
            _contextFactory.ParentCategoryId, "Test category");
        
        // Act
        var id = await handler.Handle(query, CancellationToken.None);
        
        // Assert
        var category = await Context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        
        category.ShouldNotBeNull();
        category.ParentCategoryId.ShouldBe(query.ParentCategoryId);
        category.Name.ShouldBe(query.Name);
    }

    [Fact]
    public async Task Create_InvalidParentId_ThrowsNotFoundException()
    {
        // Arrange
        await _contextFactory.SetUpForCreation(Context);

        var handler = new CreateCategory.Handler(Context);
        var query = new CreateCategory.Command(Guid.NewGuid(), "Test category");
        
        // Act & Assert
        var exception = await Should.ThrowAsync<NotFoundException>( async () =>
            await handler.Handle(query, CancellationToken.None));
        exception.ShouldNotBeNull();
        exception.EntityId.ShouldBe(query.ParentCategoryId);
        exception.EntityName.ShouldBe(nameof(Category));
    }
}