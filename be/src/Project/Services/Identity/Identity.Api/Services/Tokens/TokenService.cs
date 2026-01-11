using IdentityServer4;
using System.Threading.Tasks;
using Foundation.Account.Definitions;
using System;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Identity.Api.Infrastructure.Accounts.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using Identity.Api.Repositories.AppSecrets;
using Identity.Api.Services.Organisations;
using Foundation.Extensions.Exceptions;
using System.Net;

namespace Identity.Api.Services.Tokens
{
    public class TokenService : ITokenService
    {
        private readonly IAppSecretRepository appSecretRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IdentityServerTools tools;
        private readonly IOrganisationService organisationService;

        public TokenService(
            IAppSecretRepository appSecretRepository, 
            UserManager<ApplicationUser> userManager, 
            IdentityServerTools tools,
            IOrganisationService organisationService)
        {
            this.appSecretRepository = appSecretRepository;
            this.userManager = userManager;
            this.tools = tools;
            this.organisationService = organisationService;
        }

        public async Task<string> GetTokenAsync(string email, Guid organisationId, string appSecret)
        {
            var organisationAppSecret = await this.appSecretRepository.GetOrganisationAppSecretAsync(organisationId, appSecret);

            if (organisationAppSecret is null)
            {
                return default;
            }

            var user = await this.userManager.FindByEmailAsync(email);

            if (user is null || user.OrganisationId != organisationId)
            {
                return default;
            }

            var claims = new HashSet<Claim>(new ClaimComparer())
            {
                new Claim(AccountConstants.Claims.OrganisationIdClaim, user.OrganisationId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtClaimTypes.Audience, AccountConstants.Audiences.All)
            };

            if (await this.organisationService.IsSellerAsync(user.OrganisationId))
            {
                claims.Add(new Claim(JwtClaimTypes.Role, AccountConstants.Roles.Seller));
            }

            var token = await this.tools.IssueJwtAsync(AccountConstants.TokenLifetimes.DefaultTokenLifetimeInSeconds, claims);

            return token;
        }
    }
}