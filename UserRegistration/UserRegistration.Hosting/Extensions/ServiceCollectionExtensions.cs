using System.Linq;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace UserRegistration.Hosting.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader()
            );
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = false;
        });

        return services;
    }
    
    internal static ApiVersionSet CreateApiVersionSet(this WebApplication app)
    {
        return app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .Build();
    }
    
    internal static RouteGroupBuilder MapApi(this WebApplication app, ApiVersionSet versionSet)
    {
        return app.MapGroup("/api")
            .WithApiVersionSet(versionSet)
            .WithTags("API");
    }

    internal static IServiceCollection AddSwaggerGen(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            using var serviceProvider = services.BuildServiceProvider();
            var provider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "User Registration API",
                        Version = description.ApiVersion.ToString(),
                        Description = "API for User Registration with Event Sourcing"
                    });
            }
        });

        return services;
    }
    
    internal static WebApplication UseSwaggerUi(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            return app;
        }
        
        app.UseSwagger(options =>
        {
            options.RouteTemplate = "swagger/{documentName}/swagger.json";
        });

        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        
        app.UseSwaggerUI(options =>
        {
            foreach (var description in provider.ApiVersionDescriptions.Reverse())
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    $"User Registration API {description.GroupName.ToUpperInvariant()}");
            }
            
            options.RoutePrefix = "swagger";
        });

        return app;
    }
}
