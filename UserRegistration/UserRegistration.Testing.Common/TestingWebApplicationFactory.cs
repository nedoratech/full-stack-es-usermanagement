using Microsoft.AspNetCore.Mvc.Testing;

namespace UserRegistration.Testing.Common;

public class TestingWebApplicationFactory : WebApplicationFactory<TestStartup>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
    }
}
