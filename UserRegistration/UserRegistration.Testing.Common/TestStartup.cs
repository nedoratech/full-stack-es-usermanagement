using UserRegistration.Hosting.Extensions;
using UserRegistration.Testing.Common.Fakes;
using UserRegistration.UserManagement;
using UserRegistration.UserManagement.Abstractions;
using UserRegistration.UserManagement.ManageUser.Extensions;

namespace UserRegistration.Testing.Common;
public sealed class TestStartup
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddUrlApiVersioning()
            .AddSwaggerGen()
            .AddUserAccount()
            .AddSingleton<IEventStreamStorage, FakeEventStreamStorage>();

        var app = builder.Build();

        var apiVersionSet = app.CreateApiVersionSet();
        var api = app.MapApi(apiVersionSet);
        api.MapManageUserApi();

        app.UseSwaggerUi();
        app.UseHttpsRedirection();

        app.Run();
    }
}

