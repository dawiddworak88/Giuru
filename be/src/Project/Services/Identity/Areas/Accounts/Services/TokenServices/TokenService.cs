using Foundation.ApiExtensions.Definitions;
using IdentityModel;
using IdentityServer4;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Foundation.Account.Definitions;

namespace Identity.Api.Areas.Accounts.Services.TokenServices
{
    public class TokenService : ITokenService
    {
        private readonly IdentityServerTools tools;

        public TokenService(IdentityServerTools tools)
        {
            this.tools = tools;
        }

        public async Task<string> GetTokenAsync(string email, string password)
        {
            //var user = await this.userService.ValidateAsync(email, password);

            //if (user != null)
            //{
            //    var claims = new HashSet<Claim>(new ClaimComparer())
            //    {
            //        new Claim(AccountConstants.SellerIdClaim, user.Seller.Id.ToString()),
            //        new Claim(ClaimTypes.Email, user.Email),
            //        new Claim(ClaimTypes.Name, user.Seller.Name),
            //        new Claim(JwtClaimTypes.Audience, ApiExtensionsConstants.AllScopes)
            //    };

            //    var token = await this.tools.IssueJwtAsync(AccountConstants.DefaultTokenLifetimeInSeconds, claims);

            //    return token;
            //}

            return default;
        }
    }
}