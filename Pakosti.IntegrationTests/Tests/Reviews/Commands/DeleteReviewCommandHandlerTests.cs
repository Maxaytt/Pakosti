using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Features.Reviews.Commands;
using Pakosti.Domain.Entities;
using Pakosti.IntegrationTests.Common;
using Pakosti.IntegrationTests.Common.CqrsFactories;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.Reviews.Commands;

public class DeleteReviewCommandHandlerTests : TestCommandBase
{
    private readonly ReviewContextFactory _contextFactory = new();

    [Fact]
    public async Task Delete_ValidCommand_ReviewDeleted()
    {
        // Arrange
        await _contextFactory.SetUpForDelete(Context);

        var handler = new DeleteReview.Handler(Context);
        var command = new DeleteReview.Command(
            _contextFactory.ReviewIdForDelete,
            ContextFactory.UserAId);
        
        // Act
        await handler.Handle(command, CancellationToken.None);
        
        // Assert
        var deletedProduct = await Context.Reviews
            .FirstOrDefaultAsync(r => r.Id == command.Id, CancellationToken.None);
        deletedProduct.ShouldBeNull();
    }

    [Fact]
    public async Task Delete_InvalidReviewId_ThrowsNotFoundException()
    {
        // Arrange
        var handler = new DeleteReview.Handler(Context);
        var command = new DeleteReview.Command(
            Guid.NewGuid(),
            ContextFactory.UserAId);
        
        // Act & Assert
        var exception = await Should.ThrowAsync<NotFoundException>(async () =>
            await handler.Handle(command, CancellationToken.None));
        exception.EntityId.ShouldBe(command.Id);
        exception.EntityName.ShouldBe(nameof(Review));
    }
}