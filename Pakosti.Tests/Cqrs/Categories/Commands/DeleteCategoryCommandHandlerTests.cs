using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Cqrs.Categories.Commands.DeleteCategory;
using Pakosti.Tests.Common;
using Pakosti.Tests.Common.CqrsFactories;
using Shouldly;
using Xunit;

namespace Pakosti.Tests.Cqrs.Categories.Commands;

public class DeleteCategoryCommandHandlerTests : TestCommandBase
{
    private readonly CategoryContextFactory _contextFactory = new();

    [Fact]
    public async Task Delete_ValidCommand_CategoryDeleted()
    {
        // Arrange
        await _contextFactory.SetUpForDelete(Context);

        var handler = new DeleteCategoryCommandHandler(Context);
        var query = new DeleteCategoryCommand
        {
            Id = _contextFactory.CategoryIdForDelete
        };
        
        // Act
        await handler.Handle(query, CancellationToken.None);
        
        // Assert
        var deletedProduct = await Context.Categories
            .FirstOrDefaultAsync(c => c.Id == query.Id);
        var childCategory = await Context.Categories
            .FirstOrDefaultAsync(c => c.Id == _contextFactory.ChildCategoryId, CancellationToken.None);
        deletedProduct.ShouldBeNull();
        childCategory.ShouldNotBeNull();
        childCategory.ParentCategoryId.ShouldBeNull();
    }

    [Fact]
    public async Task Delete_InvalidCategoryId_ThrowsNotFoundException()
    {
        // Arrange
        await _contextFactory.SetUpForDelete(Context);

        var handler = new DeleteCategoryCommandHandler(Context);
        var query = new DeleteCategoryCommand
        {
            Id = Guid.NewGuid()
        };
        
        // Act & Assert
        var exception = await Should.ThrowAsync<NotFoundException>(async () =>
            await handler.Handle(query, CancellationToken.None));
        exception.EntityId.ShouldBe(query.Id);
        exception.EntityName.ShouldBe(nameof(Categories));
    }
}