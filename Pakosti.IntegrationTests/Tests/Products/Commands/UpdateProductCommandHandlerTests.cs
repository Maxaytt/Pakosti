using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Cqrs.Products.Commands.UpdateProduct;
using Pakosti.Domain.Entities;
using Pakosti.IntegrationTests.Common;
using Pakosti.IntegrationTests.Common.CqrsFactories;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.Products.Commands;

public class UpdateProductCommandHandlerTests : TestCommandBase
{
    private readonly ProductContextFactory _contextFactory = new();
    
    [Fact]
    public async Task Update_ValidCommand_ProductUpdated()
    {
        // Arrange
        await _contextFactory.SetUpForUpdate(Context);
        
        var handler = new UpdateProductCommandHandler(Context);
        
        var command = new UpdateProductCommand
        {
            Id = _contextFactory.ProductIdForUpdate,
            UserId = ContextFactory.UserAId,
            CategoryId = _contextFactory.CategoryId,
            Name = "Updated Product",
            Description = "Updated Description"
        };

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        var updatedProduct = await Context.Products.FindAsync(command.Id);
        updatedProduct.ShouldNotBeNull();
        updatedProduct.Id.ShouldBe(command.Id);
        updatedProduct.UserId.ShouldBe(command.UserId);
        updatedProduct.CategoryId.ShouldBe(command.CategoryId);
        updatedProduct.Name.ShouldBe(command.Name);
        updatedProduct.Description.ShouldBe(command.Description);
        updatedProduct.CreationDate.Date.Year.ShouldBeLessThan(DateTime.Now.Year + 1);
        updatedProduct.EditionDate.ShouldNotBeNull();
    }

    [Fact]
    public async Task Update_ProductNotFound_ThrowsNotFoundException()
    {
        // Arrange
        await _contextFactory.SetUpForUpdate(Context);

        var handler = new UpdateProductCommandHandler(Context);

        var command = new UpdateProductCommand
        {
            Id = Guid.NewGuid(),
            UserId = ContextFactory.UserAId,
            CategoryId = _contextFactory.CategoryId,
            Name = "Updated product",
            Description = "Update description"
        };

        // Act & Assert
        var exception = await Should.ThrowAsync<NotFoundException>(() => 
            handler.Handle(command, CancellationToken.None));
        exception.EntityName.ShouldBe(nameof(Product));
        exception.EntityId.ShouldBe(command.Id);
    }
    
    [Fact]
    public async Task Update_CategoryNotFound_ThrowsNotFoundException()
    {
        // Arrange
        await _contextFactory.SetUpForUpdate(Context);

        var handler = new UpdateProductCommandHandler(Context);

        var command = new UpdateProductCommand
        {
            Id = _contextFactory.ProductIdForUpdate,
            UserId = ContextFactory.UserAId,
            CategoryId = Guid.NewGuid(),
            Name = "Updated product",
            Description = "Update description"
        };

        // Act & Assert
        var exception = await Should.ThrowAsync<NotFoundException>(() => 
            handler.Handle(command, CancellationToken.None));
        exception.EntityName.ShouldBe(nameof(Category));
        exception.EntityId.ShouldBe(command.CategoryId);
    }
}