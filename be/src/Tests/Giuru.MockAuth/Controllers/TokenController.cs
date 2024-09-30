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

        [HttpGet]
        public async Task<IActionResult> GenerateToken()
        {
            var claims = new HashSet<Claim>(new ClaimComparer())
            {
                new Claim(ClaimTypes.Email, _configuration.GetValue<string>("EmailClaim")),
                new Claim(JwtClaimTypes.Audience, _configuration.GetValue<string>("Audience")),
                new Claim(JwtClaimTypes.Role, _configuration.GetValue<string>("RolesClaim")),
                new Claim(AuthConstants.OrganisationClaim, _configuration.GetValue<string>("OrganisationId"))
            };

            return StatusCode((int)HttpStatusCode.OK, new
            {
                Token = await _identityServerTools.IssueJwtAsync(_configuration.GetValue<int>("ExpiresInMinutes"), claims)
            });
        }
    }
}
