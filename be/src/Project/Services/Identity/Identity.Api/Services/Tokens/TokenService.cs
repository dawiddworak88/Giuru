using IdentityServer4;
using System.Threading.Tasks;
using Foundation.Account.Definitions;
using System;
using Foundation.ApiExtensions.Definitions;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Identity.Api.Infrastructure.Accounts.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using Identity.Api.Repositories.AppSecrets;

namespace Identity.Api.Services.Tokens
{
    public class TokenService : ITokenService
    {
        private readonly IAppSecretRepository appSecretRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IdentityServerTools tools;

        public TokenService(IAppSecretRepository appSecretRepository, UserManager<ApplicationUser> userManager, IdentityServerTools tools)
        {
            this.appSecretRepository = appSecretRepository;
            this.userManager = userManager;
            this.tools = tools;
        }

        public async Task<string> GetTokenAsync(string email, Guid organisationId, string appSecret)
        {
            var organisationAppSecret = await this.appSecretRepository.GetOrganisationAppSecretAsync(organisationId, appSecret);

            if (organisationAppSecret != null)
            {
                var user = await this.userManager.FindByEmailAsync(email);

                if (user != null)
                {
                    var claims = new HashSet<Claim>(new ClaimComparer())
                    {
                        new Claim(AccountConstants.OrganisationIdClaim, user.OrganisationId.ToString()),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(JwtClaimTypes.Audience, ApiExtensionsConstants.AllScopes)
                    };

                    var token = await this.tools.IssueJwtAsync(AccountConstants.DefaultTokenLifetimeInSeconds, claims);

                    return token;
                }
            }

            return default;
        }
    }
}