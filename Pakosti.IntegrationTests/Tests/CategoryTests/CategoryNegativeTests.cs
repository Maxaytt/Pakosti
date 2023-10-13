using System.Net;
using System.Net.Http.Json;
using Pakosti.Application.Features.Administrator.Categories.Commands;
using Pakosti.IntegrationTests.Attributes;
using Pakosti.IntegrationTests.Services;
using Shouldly;
using Testcontainers.PostgreSql;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.CategoryTests;

public class CategoryNegativeTests
{
    [Theory(Timeout = 5000), TestSetup]
    public async Task CreateCategory_InvalidParentCategoryId_ReturnsNotFound(HttpClient client)
    {
        await TestRequestService.RegisterUser(client);
        var id = Guid.NewGuid();
        var request = new CreateCategory.Command(id, "test");
        
        var response = await client.PostAsJsonAsync("/api/category", request);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task DeleteCategory_InvalidId_ReturnsNotFound(HttpClient client)
    {
        await TestRequestService.RegisterUser(client);
        var id = Guid.NewGuid();
        var response = await client.DeleteAsync($"/api/category/{id}");

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task UpdateCategory_InvalidId_ReturnsNotFound(HttpClient client)
    {
        await TestRequestService.RegisterUser(client);
        var request = new UpdateCategory.Command(Guid.NewGuid(),null ,"UpdatedCategoryName");
        
        var response = await client.PutAsJsonAsync("/api/category", request);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task GetCategory_InvalidId_ReturnsNotFound(HttpClient client)
    {
        var id = Guid.NewGuid();
        var response = await client.GetAsync($"/api/categories/{id}");

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task GetCategoryList_DatabaseUnavailable_ReturnsServerError(HttpClient client, PostgreSqlContainer container)
    {
        await container.StopAsync();
        
        var response = await client.GetAsync("/api/category");

        response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
    }
}