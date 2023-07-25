using AutoFixture;
using AutoFixture.Xunit2;
using Pakosti.IntegrationTests.Setups;

namespace Pakosti.IntegrationTests.Attributes;

public class TestSetupAttribute : AutoDataAttribute
{
    public TestSetupAttribute() : base(() => new Fixture()
        .Customize(new PostgresSetup())
        .Customize(new TestWebApplicationSetup())) { }
}