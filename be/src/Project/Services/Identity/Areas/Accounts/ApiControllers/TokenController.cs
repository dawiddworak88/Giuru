using Identity.Api.Areas.Accounts.Models;
using Feature.Account.Services.TokenServices;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : BaseApiController
    {
        private readonly ITokenService tokenService;
        private readonly ILogger logger;

        public TokenController(ITokenService tokenService, ILogger<TokenController> logger)
        {
            this.tokenService = tokenService;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateToken([FromBody] GetTokenModel model)
        {
            try
            {
                var token = await this.tokenService.GetTokenAsync(model.Email, model.Password);

                if (!string.IsNullOrWhiteSpace(token))
                {
                    return this.Ok(token);
                }

                return this.StatusCode((int)HttpStatusCode.Unauthorized);
            }
            catch (Exception exception)
            {
                var error = ErrorHelper.GenerateErrorSignature(Assembly.GetExecutingAssembly().ToString());
                this.logger.LogError(exception, $"{error.ErrorId} - {error.ErrorSource}");
                return this.StatusCode((int)HttpStatusCode.Unauthorized, $"{error.ErrorId} - {error.ErrorSource}");
            }
        }
    }
}
