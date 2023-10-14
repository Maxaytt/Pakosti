using AutoFixture;
using Pakosti.Api;
using Pakosti.IntegrationTests.Services;

namespace Pakosti.IntegrationTests.Setups;

public class TestWebApplicationSetup : ICustomization
{
    public void Customize(IFixture fixture)
    {
        var client = new TestWebApplicationFactory<Startup>(fixture).CreateClient();
        fixture.Inject(client);
    }
}