using Foundation.ApiExtensions.Definitions;
using IdentityModel;
using IdentityServer4;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Foundation.Account.Definitions;
using Identity.Api.Areas.Accounts.Services.UserServices;

namespace Identity.Api.Areas.Accounts.Services.TokenServices
{
    public class TokenService : ITokenService
    {
        private readonly IUserService userService;
        private readonly IdentityServerTools tools;

        public TokenService(IUserService userService, IdentityServerTools tools)
        {
            this.userService = userService;
            this.tools = tools;
        }

        public async Task<string> GetTokenAsync(string email, string appsecret)
        {
            return default;
        }
    }
}