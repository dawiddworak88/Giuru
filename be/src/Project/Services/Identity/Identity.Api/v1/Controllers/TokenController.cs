using Foundation.ApiExtensions.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Identity.Api.ServicesModels.Tokens;
using Identity.Api.Services.Tokens;

namespace Identity.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class TokenController : BaseApiController
    {
        private readonly ITokenService tokenService;

        public TokenController(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        /// <summary>
        /// Generates a token to use APIs.
        /// </summary>
        /// <param name="model">The credentials to obtain the token.</param>
        /// <returns>The token.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
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
