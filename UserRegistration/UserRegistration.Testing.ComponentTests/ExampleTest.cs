using FluentAssertions;
using UserRegistration.Testing.Common;
using UserRegistration.Testing.Common.Builders;
using UserRegistration.WebApi.Contracts.Request;
using UserRegistration.WebApi.Contracts.Shared;
using Xunit.Abstractions;

namespace UserRegistration.Testing.ComponentTests;

public sealed class CreateUserTests(TestingWebApplicationFactory factory, ITestOutputHelper output)
    : WebApiTestFixture(factory, output)
{
    [Fact]
    public async Task CreateUserTest()
    {
        var response = await UserManagementClient.CrateUser(CreateUserRequestBuilder.ValidEventStream());

        response.Success.Should().BeTrue();
    }
}
