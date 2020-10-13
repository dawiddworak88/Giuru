using Identity.Api.v1.Areas.Accounts.Models;
using Identity.Api.v1.Areas.Accounts.Services.TokenServices;
using Foundation.ApiExtensions.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Identity.Api.v1.Areas.Accounts.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [AllowAnonymous]
    [ApiController]
    public class TokenApiController : BaseApiController
    {
        private readonly ITokenService tokenService;

        public TokenApiController(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        /// <summary>
        /// Generates a token to use APIs.
        /// </summary>
        /// <param name="model">The credentials to obtain the token.</param>
        /// <returns>The token.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GenerateToken([FromBody] GetTokenModel model)
        {
            var token = await this.tokenService.GetTokenAsync(model.Email, model.OrganisationId, model.AppSecret);

            if (!string.IsNullOrWhiteSpace(token))
            {
                return this.Ok(token);
            }

            return this.StatusCode((int)HttpStatusCode.Unauthorized);
        }
    }
}
