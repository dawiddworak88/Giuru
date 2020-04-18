using Feature.Account.Definitions;
using Feature.Account.Services.UserServices;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Feature.Account.Services.ProfileServices
{
    public class ProfileService : IProfileService
    {
        private readonly IUserService userService;

        public ProfileService(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject?.GetSubjectId();

            if (!string.IsNullOrWhiteSpace(sub))
            {
                var user = await this.userService.FindByIdAsync(sub);

                if (user != null && user.Tenant != null && user.EmailConfirmed && user.Tenant.IsActive)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(AccountConstants.TenantIdClaim, user.Tenant.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Name, user.Tenant.Key)
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
                var user = await this.userService.FindByIdAsync(sub);

                if (user != null && user.Tenant != null && user.EmailConfirmed && user.Tenant.IsActive)
                {
                    context.IsActive = true;
                    return;
                }
            }

            context.IsActive = false;
        }
    }
}
