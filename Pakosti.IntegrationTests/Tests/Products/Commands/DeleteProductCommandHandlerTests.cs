using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Features.Products.Commands;
using Pakosti.Domain.Entities;
using Pakosti.IntegrationTests.Common;
using Pakosti.IntegrationTests.Common.CqrsFactories;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.Products.Commands;

public class DeleteProductCommandHandlerTests : TestCommandBase
{
    private readonly ProductContextFactory _contextFactory = new();

    [Fact]
    public async Task Delete_ValidCommand_ProductDeleted()
    {
        // Arrange
        await _contextFactory.SetUpForDelete(Context);

        var handler = new DeleteProduct.Handler(Context);
        var command = new DeleteProduct.Command(
            _contextFactory.ProductIdForDelete,
            ContextFactory.UserAId);
        
        // Act
        await handler.Handle(command, CancellationToken.None);
        
        // Assert
        (await Context.Products.FindAsync(command.Id))
            .ShouldBeNull();
        Context.Reviews
            .Where(r => r.ProductId == command.Id)
            .ShouldBeEmpty();
    }

    [Fact]
    public async Task Delete_ProductNotFound_ThrowsNotFoundException()
    {
        // Arrange
        await _contextFactory.SetUpForDelete(Context);

        var handler = new DeleteProduct.Handler(Context);

        var command = new DeleteProduct.Command(
            Guid.NewGuid(),
            ContextFactory.UserAId);
        
        // Act & Assert
        var exception = await Should.ThrowAsync<NotFoundException>(() =>
            handler.Handle(command, CancellationToken.None));
        exception.EntityName.ShouldBe(nameof(Product));
        exception.EntityId.ShouldBe(command.Id);
    }
    
    // Todo: UserId check in commands
}