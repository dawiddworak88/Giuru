using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IdentityServer4;
using Giuru.MockAuth.Configurations;
using System.Security.Claims;
using Giuru.MockAuth.Definitions;
using IdentityModel;
using System.IdentityModel.Tokens.Jwt;
using System;
var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddIdentityServer()
    .AddInMemoryApiResources(IdentityServerConfig.Apis)
    .AddInMemoryClients(IdentityServerConfig.GetClients())
    .AddDeveloperSigningCredential();

var app = builder.Build();

app.MapGet("/api/token", () =>
{
    var claims = new[]
        {
            new Claim(AuthConstants.OrganisationIdClaim, builder.Configuration["OrganisationClaim"]),
            new Claim(ClaimTypes.Email, builder.Configuration["EmailClaim"]),
            new Claim(JwtClaimTypes.Role, builder.Configuration["RolesClaim"])
        };

    var token = new JwtSecurityToken(
        issuer: builder.Configuration["Issuer"],
        audience: builder.Configuration["Audience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(int.Parse(builder.Configuration["ExpiresInMinutes"])));

    return new
    {
        token = new JwtSecurityTokenHandler().WriteToken(token)
    };
});

app.UseAuthorization();

app.UseIdentityServer();

app.UseStaticFiles();

app.Run();
