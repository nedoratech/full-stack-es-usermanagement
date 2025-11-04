using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using UserRegistration.Storage.Contracts;
using UserRegistration.Storage.Postgres;
using UserRegistration.UserManagement.Abstractions;

namespace UserRegistration.Storage;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddStorageConfigurationSettings(
        this IServiceCollection services, 
        IConfiguration? configuration = null)
    {
        configuration ??= services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        
        services
            .AddOptions<PostgresStorageConfigurationSettings>()
            .Bind(configuration.GetSection(PostgresStorageConfigurationSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<PostgresStorageConfigurationSettings>>().Value);
        return services;
    }
    
    public static IServiceCollection AddPostgresEventStreamStorage(this IServiceCollection services)
    {
        services.AddDbContext<EventStoreDbContext>();
        services.AddScoped<IEventStreamStorage, PostgresEventStreamStorage>();
        return services;
    }
}