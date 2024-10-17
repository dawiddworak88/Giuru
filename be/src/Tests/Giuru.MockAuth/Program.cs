using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Giuru.MockAuth.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddIdentityServer(options =>
    {
        options.IssuerUri = builder.Configuration.GetValue<string>("Issuer");
    })
    .AddInMemoryApiResources(IdentityServerConfig.Apis)
    .AddInMemoryClients(IdentityServerConfig.GetClients())
    .AddDeveloperSigningCredential();

builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthorization();

app.UseIdentityServer();

app.UseStaticFiles();

app.MapControllers();

app.Run();
