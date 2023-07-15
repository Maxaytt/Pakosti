using AutoFixture;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Pakosti.IntegrationTests.Services;

namespace Pakosti.IntegrationTests.Setups;

public class TestWebApplicationSetup : ICustomization
{
    public void Customize(IFixture fixture)
    {
        var client = new TestWebApplicationFactory<Program>(fixture).CreateClient();
        fixture.Inject(client);
    }
}