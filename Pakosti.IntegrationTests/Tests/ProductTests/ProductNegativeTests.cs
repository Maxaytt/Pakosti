using System.Net;
using System.Net.Http.Json;
using Pakosti.Application.Features.Products.Commands;
using Pakosti.IntegrationTests.Attributes;
using Pakosti.IntegrationTests.Services;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.ProductTests;

public class ProductNegativeTests
{
    [Theory(Timeout = 5000), TestSetup]
    public async Task CreateProduct_InvalidCategoryId_ReturnsNotFound(HttpClient client)
    {
        // Arrange
        await TestDataInitializer.RegisterUser(client);
        var invalidId = Guid.NewGuid();
        var request = new CreateProduct.Dto(invalidId, "test test", "test test test test!");

        // Act
        var response = await client.PostAsJsonAsync("/api/product", request);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Theory(Timeout = 5000), TestSetup]
    public async Task UpdateProduct_InvalidId_ReturnsNotFound(HttpClient client)
    {
        // Arrange
        await TestDataInitializer.RegisterUser(client);
        var request = new UpdateProduct.Dto(Guid.NewGuid(), Guid.NewGuid(), null, null);
        
        // Act
        var response = await client.PutAsJsonAsync("/api/product", request);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Theory(Timeout = 5000), TestSetup]
    public async Task DeleteProduct_InvalidId_ReturnsNotFound(HttpClient client)
    {
        // Arrange
        await TestDataInitializer.RegisterUser(client);
        var id = Guid.NewGuid();
        // Act
        var response = await client.DeleteAsync($"/api/product/{id}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Theory(Timeout = 5000), TestSetup]
    public async Task GetProduct_InvalidId_ReturnsNotFound(HttpClient client)
    {
        // Arrange
        var id = Guid.NewGuid();
        
        // Act
        var response = await client.GetAsync($"/api/product/{id}");
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}