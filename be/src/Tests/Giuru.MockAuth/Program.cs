using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Giuru.MockAuth.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddIdentityServer(options =>
    {
        options.IssuerUri = "null";
    })
    .AddInMemoryApiResources(IdentityServerConfig.Apis)
    .AddInMemoryClients(IdentityServerConfig.GetClients())
    .AddDeveloperSigningCredential();

builder.Services.AddControllers();

builder.Services.AddAuthentication()
    .AddIdentityServerAuthentication("IsToken", options =>
    {
        options.Authority = builder.Configuration.GetValue<string>("IdentityUrl");
        options.RequireHttpsMetadata = false;
        options.ApiName = "all";
    });

var app = builder.Build();

app.UseAuthorization();

app.UseIdentityServer();

app.UseStaticFiles();

app.MapControllers();

app.Run();
