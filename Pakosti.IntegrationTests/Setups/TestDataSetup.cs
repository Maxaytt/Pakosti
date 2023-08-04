using AutoFixture;
using Pakosti.Application.Features.Categories.Commands;
using Pakosti.Application.Features.Identities.Commands;

namespace Pakosti.IntegrationTests.Setups;

public class TestDataSetup : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Register(() => new Register.Command(
            "testEmail@example.com",
            DateTime.Today.AddYears(-20),
            "passwordQ1!",
            "passwordQ1!",
            "TestFirstName",
            "TestLastName",
            "TestUsername"
        ));
        fixture.Register(() => new CreateCategory.Command(null, "test"));
    }
}