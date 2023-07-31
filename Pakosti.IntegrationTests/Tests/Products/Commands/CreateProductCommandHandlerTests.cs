using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Features.Products.Commands;
using Pakosti.IntegrationTests.Common;
using Pakosti.IntegrationTests.Common.CqrsFactories;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.Products.Commands;

public class CreateProductCommandHandlerTests : TestCommandBase
{
    private readonly ProductContextFactory _contextFactory = new();
        
    [Fact]
    public async Task Create_NoCategory_ProductAddedToDatabase()
    {
        // Arrange
        var handler = new CreateProduct.Handler(Context);
        var query = new CreateProduct.Command(
            ContextFactory.UserAId,
            null,
            "Test product",
            "bal bal bal");
        
        // Act
        var resultId = await handler.Handle(query, CancellationToken.None);
        
        // Assert
        resultId.ShouldNotBe(Guid.Empty);
        var createdProduct = await Context.Products
            .FirstOrDefaultAsync(p => p.Id == resultId, CancellationToken.None);
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
        var handler = new CreateProduct.Handler(Context);
        var query = new CreateProduct.Command(
            ContextFactory.UserAId,
            _contextFactory.CategoryId,
            "Test product",
            "bal bal bal");
        
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
        var handler = new CreateProduct.Handler(Context);
        var command = new CreateProduct.Command(
            Guid.NewGuid(),
            Guid.NewGuid(), // Invalid category ID
            "Test Product",
            "Test Description");

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
    }
}