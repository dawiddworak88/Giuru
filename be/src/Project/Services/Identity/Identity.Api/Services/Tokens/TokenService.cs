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
using Feature.Account;
using Foundation.Extensions.Exceptions;
using Microsoft.Extensions.Localization;

namespace Identity.Api.Services.Tokens
{
    public class TokenService : ITokenService
    {
        private readonly IAppSecretRepository appSecretRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IdentityServerTools tools;
        private readonly IOrganisationService organisationService;
        private readonly IStringLocalizer<AccountResources> accountLocalizer;

        public TokenService(
            IAppSecretRepository appSecretRepository,
            UserManager<ApplicationUser> userManager,
            IdentityServerTools tools,
            IOrganisationService organisationService,
            IStringLocalizer<AccountResources> accountLocalizer)
        {
            this.appSecretRepository = appSecretRepository;
            this.userManager = userManager;
            this.tools = tools;
            this.organisationService = organisationService;
            this.accountLocalizer = accountLocalizer;
        }

        public async Task<string> GetTokenAsync(string email, Guid organisationId, string appSecret)
        {
            var organisationAppSecret = await this.appSecretRepository.GetOrganisationAppSecretAsync(organisationId, appSecret);

            if (organisationAppSecret is null)
            {
                return default;
            }

            var user = await this.userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return default;
            }

            if (user.IsDisabled || await this.organisationService.IsDisabledAsync(user.OrganisationId))
            {
                throw new ConflictException(this.accountLocalizer.GetString("AccountIsInactive"));
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

            return await this.tools.IssueJwtAsync(AccountConstants.TokenLifetimes.DefaultTokenLifetimeInSeconds, claims);
        }
    }
}