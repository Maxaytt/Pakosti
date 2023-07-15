using AutoFixture;
using AutoFixture.Xunit2;
using Pakosti.IntegrationTests.Setups;

namespace Pakosti.IntegrationTests.Attributes;

public class ApiSetupAttribute : AutoDataAttribute
{
    public ApiSetupAttribute() : base(() => new Fixture()
        .Customize(new SqlServerSetup())
        .Customize(new TestWebApplicationSetup())) { }
}