using IdentityModel;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public TokenController(IdentityServerTools identityServerTools)
        {
            _identityServerTools = identityServerTools;
        }

        [HttpGet]
        public async Task<IActionResult> GenerateToken()
        {
            var claims = new HashSet<Claim>(new ClaimComparer())
            {
                new Claim(ClaimTypes.Email, "test@test.pl"),
                new Claim(JwtClaimTypes.Audience, "all"),
                new Claim(JwtClaimTypes.Role, "Seller"),
                new Claim("OrganisationId", "09affcc9-1665-45d6-919f-3d2026106ba1")
            };

            return StatusCode((int)HttpStatusCode.OK, new
            {
                Token = await _identityServerTools.IssueJwtAsync(86400, claims)
            });
        }
    }
}
