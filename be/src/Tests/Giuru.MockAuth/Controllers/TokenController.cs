using Giuru.MockAuth.Definitions;
using IdentityModel;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Giuru.MockAuth.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IdentityServerTools _identityServerTools;
        private readonly IConfiguration _configuration;

        public TokenController(
            IdentityServerTools identityServerTools,
            IConfiguration configuration)
        {
            _identityServerTools = identityServerTools;
            _configuration = configuration;
        }

        /// <summary>
        /// Issues a token for the configured identity. The optional query values issue one for another identity instead,
        /// so a test can act as a second seller or as a buyer without a role. An empty role means no role claim.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GenerateToken(string email = null, string role = null, string organisationId = null)
        {
            var claims = new HashSet<Claim>(new ClaimComparer())
            {
                new Claim(ClaimTypes.Email, string.IsNullOrWhiteSpace(email) ? _configuration.GetValue<string>("EmailClaim") : email),
                new Claim(JwtClaimTypes.Audience, _configuration.GetValue<string>("Audience")),
                new Claim(AuthConstants.OrganisationClaim, string.IsNullOrWhiteSpace(organisationId) ? _configuration.GetValue<string>("OrganisationId") : organisationId)
            };

            // Not specifying a role keeps the configured one; asking for "none" issues a token without any role.
            var effectiveRole = role is null ? _configuration.GetValue<string>("RolesClaim") : role;

            if (string.IsNullOrWhiteSpace(effectiveRole) is false && string.Equals(effectiveRole, "none", System.StringComparison.OrdinalIgnoreCase) is false)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, effectiveRole));
            }

            return StatusCode((int)HttpStatusCode.OK, new
            {
                Token = await _identityServerTools.IssueJwtAsync(_configuration.GetValue<int>("ExpiresInMinutes"), claims)
            });
        }
    }
}
