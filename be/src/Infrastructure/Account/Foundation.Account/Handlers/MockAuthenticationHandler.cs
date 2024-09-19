using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Foundation.Account.Handlers
{
    public class MockAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public string MockAuthToken { get; set; }

        public MockAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authProperties = new AuthenticationProperties();

            authProperties.StoreTokens(new[] 
            {
                new AuthenticationToken
                {
                    Name = ApiExtensionsConstants.TokenName,
                    Value = MockAuthToken
                }
            });

            var ticket = new AuthenticationTicket(new ClaimsPrincipal(), authProperties, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
