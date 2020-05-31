using Feature.Account.Definitions;
using Feature.Account.Services.UserServices;
using Foundation.ApiExtensions.Definitions;
using IdentityModel;
using IdentityServer4;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Feature.Account.Services.TokenServices
{
    public class TokenService : ITokenService
    {
        private readonly IUserService userService;
        private readonly IdentityServerTools tools;

        public TokenService(IdentityServerTools tools, IUserService userService)
        {
            this.tools = tools;
            this.userService = userService;
        }

        public async Task<string> GetTokenAsync(string email, string password)
        {
            var user = await this.userService.ValidateAsync(email, password);

            if (user != null)
            {
                var claims = new HashSet<Claim>(new ClaimComparer())
                {
                    new Claim(AccountConstants.TenantIdClaim, user.Tenant.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Tenant.Key),
                    new Claim(JwtClaimTypes.Scope, ApiExtensionsConstants.AllScopes),
                    new Claim(JwtClaimTypes.Audience, ApiExtensionsConstants.AllScopes)
                };

                var token = await this.tools.IssueJwtAsync(AccountConstants.DefaultTokenLifetimeInSeconds, claims);

                return token;
            }

            return default;
        }
    }
}