using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Definitions;
using Identity.Api.Infrastructure.Accounts.Entities;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.Services.ProfileServices
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject?.GetSubjectId();

            if (!string.IsNullOrWhiteSpace(sub))
            {
                var user = await this.userManager.FindByIdAsync(sub);

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(AccountConstants.OrganisationIdClaim, user.OrganisationId.ToString()),
                        new Claim(ClaimTypes.Email, user.Email)
                    };

                    context.IssuedClaims.AddRange(claims);
                }
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject?.GetSubjectId();

            if (!string.IsNullOrWhiteSpace(sub))
            {
                var user = await this.userManager.FindByIdAsync(sub);

                if (user != null)
                {
                    context.IsActive = true;
                }
            }
        }
    }
}
