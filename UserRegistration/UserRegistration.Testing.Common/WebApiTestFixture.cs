using Refit;
using UserRegistration.WebApi.Client;
using Xunit;
using Xunit.Abstractions;

namespace UserRegistration.Testing.Common;

public abstract class WebApiTestFixture : IClassFixture<TestingWebApplicationFactory>, IDisposable
{
    protected readonly TestingWebApplicationFactory Factory;
    protected readonly IUserRegistrationClient UserManagementClient;
    private readonly HttpClient _client;

    protected WebApiTestFixture(TestingWebApplicationFactory factory, ITestOutputHelper output)
    {
        Factory = factory;
        
        _client = factory.CreateClient();
        UserManagementClient = RestService.For<IUserRegistrationClient>(_client);
    }
    
    protected T? GetService<T>() where T : class
    {
        return Factory.Services.GetService<T>();
    }
    
    protected T GetRequiredService<T>() where T : class
    {
        return Factory.Services.GetRequiredService<T>();
    }

    void IDisposable.Dispose()
    {
        _client?.Dispose();
    }
}

