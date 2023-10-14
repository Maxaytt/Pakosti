using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using AutoFixture;
using Pakosti.Application.Features.Administrator.Categories.Commands;
using Pakosti.Application.Features.Administrator.Categories.Queries;
using Pakosti.Application.Features.Administrator.Products.Commands;
using Pakosti.Application.Features.Consumer.Reviews.Commands;
using Pakosti.Application.Features.Guest.Identities.Commands;
using Pakosti.Application.Features.Guest.Products.Queries;
using Pakosti.Application.Features.Guest.Reviews.Queries;
using Pakosti.IntegrationTests.Setups;

namespace Pakosti.IntegrationTests.Services;

public static class TestRequestService
{
    private static readonly Fixture Fixture = new();

    static TestRequestService()
    {
        Fixture.Customize(new TestDataSetup());
    }
    
    public static async Task RegisterUser(HttpClient client)
    {
        var request = Fixture.Create<Register.Command>();
        var response = await client.PostAsJsonAsync("/api/identity/register", request);
        var responseData = await response.Content.ReadFromJsonAsync<Authenticate.Response>(); 
        client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", responseData!.Token);
    }

    public static async Task<Guid> CreateCategory(HttpClient client, 
        (Guid? parentId, string name) dto)
    {
        var request = new CreateCategory.Command(dto.parentId, dto.name);
        var response = await client.PostAsJsonAsync("/api/category", request);
        var responseData = await response.Content.ReadFromJsonAsync<CreateCategory.Response>();
        return responseData!.Id;
    }
    
    public static async Task<GetCategory.Response?> GetCategory(HttpClient client, Guid id)
    {
        var response = await client.GetAsync($"/api/category/{id}");
        var result = response.StatusCode switch
        {
            HttpStatusCode.OK =>
                await response.Content.ReadFromJsonAsync<GetCategory.Response>(),
            _ => null
        };
        return result;
    }
    
    public static async Task<Guid> CreateProduct(HttpClient client, 
        (Guid categoryId, string name, string description) dto)
    {
        var request = new CreateProduct.Dto(dto.categoryId, dto.name, dto.description, 5, "USD");
        var response = await client.PostAsJsonAsync("/api/product", request);
        var responseData = await response.Content.ReadFromJsonAsync<CreateProduct.Response>();
        return responseData!.Id;
    }

    public static async Task<GetProduct.Response?> GetProduct(HttpClient client, Guid id)
    {
        var response = await client.GetAsync($"/api/product/{id}");
        var result = response.StatusCode switch
        {
            HttpStatusCode.OK =>
                await response.Content.ReadFromJsonAsync<GetProduct.Response>(),
            _ => null
        };
        return result;
    }
    
    public static async Task<Guid> CreateReview(HttpClient client, 
        (Guid productId, string header, string body) dto)
    {
        var request = new CreateReview.Dto(dto.productId, dto.header, dto.body);
        var response = await client.PostAsJsonAsync("/api/review", request);
        var responseData = await response.Content.ReadFromJsonAsync<CreateReview.Response>();
        return responseData!.Id;
    }

    public static async Task<GetReview.Response?> GetReview(HttpClient client, Guid id)
    {
        var response = await client.GetAsync($"/api/review/{id}");
        var result = response.StatusCode switch
        {
            HttpStatusCode.OK =>
                await response.Content.ReadFromJsonAsync<GetReview.Response>(),
            _ => null
        };
        return result;
    }
}