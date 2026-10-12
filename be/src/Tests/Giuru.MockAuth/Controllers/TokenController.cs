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
        private const string NoRole = "none";

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
        /// Issues a token. Without parameters the identity comes from the configuration, which is the seller the whole
        /// test run signs in as. A test that needs another identity - a second seller, a buyer, a member of a buyer's
        /// team - overrides the parts that make it different. The role <c>none</c> issues no role at all, which is what a
        /// buyer looks like (an empty query value would be bound as a missing one and fall back to the default role).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GenerateToken(string email = null, string role = null, string organisationId = null)
        {
            var claims = new HashSet<Claim>(new ClaimComparer())
            {
                new Claim(ClaimTypes.Email, email ?? _configuration.GetValue<string>("EmailClaim")),
                new Claim(JwtClaimTypes.Audience, _configuration.GetValue<string>("Audience")),
                new Claim(AuthConstants.OrganisationClaim, organisationId ?? _configuration.GetValue<string>("OrganisationId"))
            };

            var roleClaim = role ?? _configuration.GetValue<string>("RolesClaim");

            if (string.IsNullOrWhiteSpace(roleClaim) is false && string.Equals(roleClaim, NoRole, System.StringComparison.OrdinalIgnoreCase) is false)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, roleClaim));
            }

            return StatusCode((int)HttpStatusCode.OK, new
            {
                Token = await _identityServerTools.IssueJwtAsync(_configuration.GetValue<int>("ExpiresInMinutes"), claims)
            });
        }
    }
}
