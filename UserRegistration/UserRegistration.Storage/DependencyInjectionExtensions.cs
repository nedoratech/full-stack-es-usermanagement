using Microsoft.Extensions.DependencyInjection;
using UserRegistration.Storage.Postgres;
using UserRegistration.UserManagement.Abstractions;

namespace UserRegistration.Storage;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddPostgresEventStreamStorage(this IServiceCollection services)
    {
        services.AddDbContext<EventStoreDbContext>();
        services.AddScoped<IEventStreamStorage, PostgresEventStreamStorage>();
        return services;
    }
}