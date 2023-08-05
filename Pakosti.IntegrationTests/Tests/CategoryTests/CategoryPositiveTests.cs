using System.Net;
using System.Net.Http.Json;
using Pakosti.Application.Features.Categories.Commands;
using Pakosti.Application.Features.Categories.Queries;
using Pakosti.IntegrationTests.Attributes;
using Pakosti.IntegrationTests.Services;
using Shouldly;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.CategoryTests;

public class CategoryPositiveTests
{
    private const string Name = "test test";
    private const string UpdatedName = "updated test name";
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task CreateCategory_ShouldCreate_Category(HttpClient client)
    {
        //Arrange
        await TestRequestService.RegisterUser(client);
        var request = new CreateCategory.Command(null, Name);
        
        // Act
        var response = await client.PostAsJsonAsync("/api/category", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task DeleteCategory_ShouldSetNull_ChildrenParentId(HttpClient client)
    {
        //Arrange
        await TestRequestService.RegisterUser(client);
        var id = await TestRequestService.CreateCategory(client, (null, "test1"));
        var childId = await TestRequestService.CreateCategory(client, (id, "test2"));
        

        // Act
        var response = await client.DeleteAsync($"/api/category/{id}");
        var childResponse = await client.GetAsync($"/api/category/{childId}");

        // Assert
        var childData = await childResponse.Content.ReadFromJsonAsync<GetCategory.Response>();
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        childData!.ParentCategoryId.ShouldBeNull();
        
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task UpdateCategory_ShouldUpdate_Category(HttpClient client)
    {
        //Arrange
        await TestRequestService.RegisterUser(client);
        var id = await TestRequestService.CreateCategory(client, (null, Name));
        var request = new UpdateCategory.Command(id, null,UpdatedName);
        
        // Act
        var response = await client.PutAsJsonAsync("/api/category", request);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task GetCategory_ShouldGet_Category(HttpClient client)
    {
        //Arrange
        await TestRequestService.RegisterUser(client);
        var id = await TestRequestService.CreateCategory(client, (null, "test"));

        // Act
        var response = await client.GetAsync($"/api/category/{id}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task GetCategoryList_ShouldGetAll_Categories(HttpClient client)
    {
        // Arrange
        await TestRequestService.RegisterUser(client);
        await TestRequestService.CreateCategory(client, (null, "test"));

        var response = await client.GetAsync("/api/category");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}