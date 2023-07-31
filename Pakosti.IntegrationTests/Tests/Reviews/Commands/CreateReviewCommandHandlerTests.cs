using Microsoft.EntityFrameworkCore;
using Pakosti.Application.Common.Exceptions;
using Pakosti.Application.Features.Reviews.Commands;
using Pakosti.Domain.Entities;
using Pakosti.IntegrationTests.Common;
using Pakosti.IntegrationTests.Common.CqrsFactories;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.Reviews.Commands;

public class CreateReviewHandlerTests : TestCommandBase
{
    private readonly ReviewContextFactory _contextFactory = new();

    [Fact]
    public async Task Create_ValidCommand_ReviewAddedToDatabase()
    {
        // Arrange
        await _contextFactory.SetUpForCreate(Context);

        var handler = new CreateReview.Handler(Context);
        var command = new CreateReview.Command(
            ContextFactory.UserAId,
            _contextFactory.ProductId,
            "test review",
            "created review");
        
        // Act
        var resultId = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        var review = await Context.Reviews
            .FirstOrDefaultAsync(r => r.Id == resultId, CancellationToken.None);
        
        review.ShouldNotBeNull();
        review.Header.ShouldBe(command.Header);
        review.Body.ShouldBe(command.Body);
        review.ProductId.ShouldBe(command.ProductId);
        review.UserId.ShouldBe(command.UserId);
    }

    [Fact]
    public async Task Create_InvalidProductId_ThrowsNotFoundException()
    {
        // Arrange
        var handler = new CreateReview.Handler(Context);
        var command = new CreateReview.Command(
            ContextFactory.UserAId,
            Guid.NewGuid(),
            "test review",
            "created review");
        
        // Act & Assert
        var exception = await Should.ThrowAsync<NotFoundException>(async () =>
            await handler.Handle(command, CancellationToken.None));
        exception.EntityId.ShouldBe(command.ProductId);
        exception.EntityName.ShouldBe(nameof(Product));
    }
}