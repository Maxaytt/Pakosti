using System.Net;
using System.Net.Http.Json;
using Pakosti.Application.Features.Consumer.Reviews.Commands;
using Pakosti.IntegrationTests.Attributes;
using Pakosti.IntegrationTests.Services;
using Shouldly;
using Testcontainers.PostgreSql;
using Xunit;

namespace Pakosti.IntegrationTests.Tests.ReviewTests;

public class ReviewNegativeTests
{
    private const string Header = "test test";
    private const string Body = "test test test test test!";
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task CreateReview_InvalidProductId_ReturnsNotFound(HttpClient client)
    {
        await TestRequestService.RegisterUser(client);
        var id = Guid.NewGuid();
        var request = new CreateReview.Dto(id, Header, Body);
        
                var response = await client.PostAsJsonAsync("/api/review", request);
        
                response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task UpdateReview_InvalidProductId_ReturnsNotFound(HttpClient client)
    {
        await TestRequestService.RegisterUser(client);
        var id = Guid.NewGuid();
        var request = new UpdateReview.Dto(id, Header, Body);
        
                var response = await client.PostAsJsonAsync("/api/review", request);
        
                response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task DeleteReview_InvalidProductId_ReturnsNotFound(HttpClient client)
    {
        await TestRequestService.RegisterUser(client);
        var id = Guid.NewGuid();
        
        var response = await client.DeleteAsync($"/api/review/{id}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task GetReview_InvalidProductId_ReturnsNotFound(HttpClient client)
    {
        var id = Guid.NewGuid();
        
        var response = await client.GetAsync($"/api/review/{id}");
        
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
    
    [Theory(Timeout = 5000), TestSetup]
    public async Task GetReviewList_DatabaseUnavailable_ReturnsServerError(HttpClient client,
        PostgreSqlContainer container)
    {
        await container.StopAsync();

        var response = await client.GetAsync("/api/review");
        
        response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
    }
}