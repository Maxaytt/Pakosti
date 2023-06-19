using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Cqrs.Products.Commands.CreateProduct;
using Pakosti.Tests.Common;
using Pakosti.Tests.Common.CqrsFactories;
using Shouldly;
using Xunit;

namespace Pakosti.Tests.Products.Commands;

public class CreateProductCommandHandlerTests : TestCommandBase
{
    private readonly ProductContextFactory _contextFactory = new();
        
    [Fact]
    public async Task Create_NoCategory_ProductAddedToDatabase()
    {
        // Arrange
        var handler = new CreateProductCommandHandler(Context);
        var query = new CreateProductCommand
        {
            UserId = _contextFactory.UserAId,
            CategoryId = null,
            Name = "Test product",
            Description = "bal bal bal"
        };
        
        // Act
        var resultId = await handler.Handle(query, CancellationToken.None);
        
        // Assert
        resultId.ShouldNotBe(Guid.Empty);
        var createdProduct = await Context.Products.FindAsync(resultId);
        createdProduct.ShouldNotBeNull();
        createdProduct.UserId.ShouldBe(query.UserId);
        createdProduct.CategoryId.ShouldBe(query.CategoryId);
        createdProduct.Name.ShouldBe(query.Name);
        createdProduct.Description.ShouldBe(query.Description);
    }
    
    [Fact]
    public async Task Create_WithCategory_ProductAddedToDatabase()
    {
        // Arrange
        await _contextFactory.SetUpForCreation(Context);
        var handler = new CreateProductCommandHandler(Context);
        var query = new CreateProductCommand
        {
            UserId = _contextFactory.UserAId,
            CategoryId = _contextFactory.CategoryId,
            Name = "Test product",
            Description = "bal bal bal"
        };
        
        // Act
        var resultId = await handler.Handle(query, CancellationToken.None);
        
        // Assert
        resultId.ShouldNotBe(Guid.Empty);
        var createdProduct = await Context.Products.FindAsync(resultId);
        createdProduct.ShouldNotBeNull();
        createdProduct.UserId.ShouldBe(query.UserId);
        createdProduct.CategoryId.ShouldBe(query.CategoryId);
        createdProduct.Name.ShouldBe(query.Name);
        createdProduct.Description.ShouldBe(query.Description);
    }
    
    [Fact]
    public async Task Create_InvalidCategoryId_ThrowsNotFoundException()
    {
        // Arrange
        var handler = new CreateProductCommandHandler(Context);
        var command = new CreateProductCommand
        {
            UserId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(), // Invalid category ID
            Name = "Test Product",
            Description = "Test Description"
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}