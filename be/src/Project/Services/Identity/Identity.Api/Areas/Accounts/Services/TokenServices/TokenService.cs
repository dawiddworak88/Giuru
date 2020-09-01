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
            return default;
        }
    }
}