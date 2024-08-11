using Giuru.MockAuth.Definitions;
using IdentityModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

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

app.Run();
