using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using AutoFixture;
using Pakosti.Application.Features.Categories.Commands;
using Pakosti.Application.Features.Identities.Commands;
using Pakosti.Application.Features.Products.Commands;
using Pakosti.Application.Features.Products.Queries;
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
    
    public static async Task<Guid> CreateProduct(HttpClient client, 
        (Guid categoryId, string name, string description) dto)
    {
        var request = new CreateProduct.Dto(dto.categoryId, dto.name, dto.description);
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
}