using System.Net.Http.Headers;
using System.Net.Http.Json;
using AutoFixture;
using Pakosti.Application.Features.Categories.Commands;
using Pakosti.Application.Features.Identities.Commands;
using Pakosti.IntegrationTests.Setups;

namespace Pakosti.IntegrationTests.Services;

public class TestDataInitializer
{
    private static readonly Fixture Fixture = new();

    static TestDataInitializer()
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
}