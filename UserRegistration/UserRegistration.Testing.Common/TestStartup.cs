using UserRegistration.Hosting.Extensions;
using UserRegistration.UserManagement.ManageUser.Extensions;

namespace UserRegistration.Testing.Common;
public sealed class TestStartup
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddUrlApiVersioning()
            .AddSwaggerGen();

        var app = builder.Build();

        var apiVersionSet = app.CreateApiVersionSet();
        var api = app.MapApi(apiVersionSet);
        api.MapManageUserApi();

        app.UseSwaggerUi();
        app.UseHttpsRedirection();

        app.Run();
    }
}

