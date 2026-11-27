using Foundation.Account.Definitions;
using Identity.Api.Configurations;
using Identity.Api.Infrastructure;
using Identity.Api.Infrastructure.Accounts.Entities;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.Services.ProfileServices
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IdentityContext context;
        private readonly IOptions<AppSettings> options;

        public ProfileService(
            UserManager<ApplicationUser> userManager,
            IdentityContext context,
            IOptions<AppSettings> options)
        {
            this.userManager = userManager;
            this.context = context;
            this.options = options;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject?.GetSubjectId();

            if (!string.IsNullOrWhiteSpace(sub))
            {
                var user = await this.userManager.FindByIdAsync(sub);

                if (user is not null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(AccountConstants.Claims.OrganisationIdClaim, user.OrganisationId.ToString()),
                        new Claim(ClaimTypes.Email, user.Email)
                    };

                    context.IssuedClaims.AddRange(claims);

                    var organisation = await this.context.Organisations.FirstOrDefaultAsync(x => x.Id == user.OrganisationId);

                    if (organisation is not null && organisation.IsSeller)
                    {
                        context.IssuedClaims.Add(new Claim(JwtClaimTypes.Role, AccountConstants.Roles.Seller));
                    }

                    if (string.IsNullOrWhiteSpace(user.FirstName) is false && string.IsNullOrWhiteSpace(user.LastName) is false)
                    {
                        context.IssuedClaims.Add(new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"));
                    }
                    else if (organisation is not null)
                    {
                        context.IssuedClaims.Add(new Claim(ClaimTypes.Name, organisation.Name));
                    }
                }
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject?.GetSubjectId();

            if (!string.IsNullOrWhiteSpace(sub))
            {
                var user = await this.userManager.FindByIdAsync(sub);

                if (user is null || user.IsDisabled)
                {
                    context.IsActive = false;

                    return;
                }

                var isOrganisationDisabled = await this.context.Organisations.AnyAsync(x => x.Id == user.OrganisationId && x.IsDisabled && x.IsActive);

                if (isOrganisationDisabled)
                {
                    context.IsActive = false;

                    return;
                }

                // Same rule as the Seller.Web sign-in, so refresh can't outlive the seller status
                if (Guid.TryParse(context.Client?.ClientId, out var clientId) && clientId == this.options.Value.SellerClientId)
                {
                    context.IsActive = await this.context.Organisations.AnyAsync(x => x.Id == user.OrganisationId && x.IsSeller && x.IsActive);
                }
            }
        }
    }
}
