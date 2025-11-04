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
    public async Task GivenValidUserCreationEventStream_WhenCreatingUser_ThenExpectAggregate()
    {
        var request = CreateUserRequestBuilder.ValidEventStream();
        
        var response = await UserManagementClient.CrateUser(request);

        response.Success.Should().BeTrue();
    }
}
