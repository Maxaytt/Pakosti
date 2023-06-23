using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Cqrs.Categories.Commands.UpdateCategory;
using Pakosti.Application.Interfaces;
using Pakosti.Tests.Common.CqrsFactories;
using Pakosti.Tests.Common;
using Shouldly;
using Xunit;

namespace Pakosti.Tests.Cqrs.Categories.Commands;

public class UpdateCategoryCommandHandlerTests : TestCommandBase
{
    private readonly CategoryContextFactory _contextFactory = new();

    [Fact]
    public async Task Update_WithParent_ProductUpdated()
    {
        // Arrange
        await _contextFactory.SetUpForUpdate(Context);

        var handler = new UpdateCategoryCommandHandler(Context);
        var query = new UpdateCategoryCommand
        {
            Id = _contextFactory.CategoryIdForUpdate,
            Name = "Updated name",
            ParentCategoryId = _contextFactory.ParentCategoryId
        };
        
        // Act
        await handler.Handle(query, CancellationToken.None);
        
        // Assert
        var updatedCategory = await Context.Categories
            .FirstOrDefaultAsync(c => c.Id == query.Id, CancellationToken.None);
        var parentCategory = await Context.Categories
            .FirstOrDefaultAsync(c => c.Id == query.ParentCategoryId, CancellationToken.None);
        
        updatedCategory.ShouldNotBeNull();
        updatedCategory.ParentCategoryId.ShouldBe(query.ParentCategoryId);
        updatedCategory.Name.ShouldBe(query.Name);
        
        parentCategory.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task Update_WithoutParent_ProductUpdated()
    {
        // Arrange
        await _contextFactory.SetUpForUpdate(Context);

        var handler = new UpdateCategoryCommandHandler(Context);
        var query = new UpdateCategoryCommand
        {
            Id = _contextFactory.CategoryIdForUpdate,
            Name = "Updated name",
            ParentCategoryId = null
        };
        
        // Act
        await handler.Handle(query, CancellationToken.None);
        
        // Assert
        var updatedCategory = await Context.Categories
            .FirstOrDefaultAsync(c => c.Id == query.Id, CancellationToken.None);

        updatedCategory.ShouldNotBeNull();
        updatedCategory.ParentCategoryId.ShouldBe(query.ParentCategoryId);
        updatedCategory.Name.ShouldBe(query.Name);
    }

    [Fact]
    public async Task Update_InvalidParentCategoryId_ThrowsNotFoundException()
    {
        // Arrange
        await _contextFactory.SetUpForUpdate(Context);

        var handler = new UpdateCategoryCommandHandler(Context);
        var query = new UpdateCategoryCommand
        {
            Id = _contextFactory.CategoryIdForUpdate,
            ParentCategoryId = Guid.NewGuid(),
            Name = "Invalid update"
        };
        
        // Act & Assert
        var exception = await Should.ThrowAsync<NotFoundException>( async () =>
            await handler.Handle(query, CancellationToken.None));

        exception.EntityId.ShouldBe(query.ParentCategoryId);
        exception.EntityName.ShouldBe(nameof(Categories));
    }
    
    [Fact]
    public async Task Update_InvalidCategoryId_ThrowsNotFoundException()
    {
        // Arrange
        await _contextFactory.SetUpForUpdate(Context);

        var handler = new UpdateCategoryCommandHandler(Context);
        var query = new UpdateCategoryCommand
        {
            Id = Guid.NewGuid(),
            ParentCategoryId = _contextFactory.ParentCategoryId,
            Name = "Invalid update"
        };
        
        // Act & Assert
        var exception = await Should.ThrowAsync<NotFoundException>( async () =>
            await handler.Handle(query, CancellationToken.None));

        exception.EntityId.ShouldBe(query.Id);
        exception.EntityName.ShouldBe(nameof(Categories));
    }
}