using AutoFixture;
using Nito.AsyncEx;
using Testcontainers.MsSql;

namespace Pakosti.IntegrationTests.Setups;

public class PostgresSetup : ICustomization
{
    public void Customize(IFixture fixture)
    {
        var container = new MsSqlBuilder()
            .WithExposedPort(1433)
            .WithPortBinding(1433, true)
            .Build();

        AsyncContext.Run(async () => await container.StartAsync());
        fixture.Inject(container);
    }
}