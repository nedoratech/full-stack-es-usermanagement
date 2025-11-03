using Microsoft.Extensions.DependencyInjection;
using UserRegistration.UserManagement.Abstractions;

namespace UserRegistration.UserManagement;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddUserAccount(this IServiceCollection services)
    {
        services
            .AddScoped<IUserAccount, UserAccount>()
            .AddScoped<IUserAccountRepository, UserAccountRepository>();
        return services;
    }
}