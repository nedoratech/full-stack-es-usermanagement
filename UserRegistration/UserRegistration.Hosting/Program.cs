using System;
using Microsoft.AspNetCore.Builder;
using UserRegistration.Hosting.Extensions;
using UserRegistration.Storage;
using UserRegistration.UserManagement.ManageUser.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddUrlApiVersioning()
    .AddSwaggerGen()
    .AddPostgresEventStreamStorage();

var app = builder.Build();
var apiVersionSet = app.CreateApiVersionSet();
var api = app.MapApi(apiVersionSet);
api.MapManageUserApi();

app.UseSwaggerUi();
app.UseHttpsRedirection();

try
{
    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Application terminated unexpectedly: {ex.Message}");
    throw;
}
