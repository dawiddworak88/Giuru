using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.Helpers;
using Identity.Api.Services.Secrets;
using Identity.Api.ServicesModels.Secrets;
using Identity.Api.Validators.Secrets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = "IsToken")]
    [ApiController]
    public class SecretsController : BaseApiController
    {
        private readonly ISecretsService secretsService;

        public SecretsController(
            ISecretsService secretsService)
        {
            this.secretsService = secretsService;
        }

        /// <summary>
        /// Get an secret app.
        /// </summary>
        /// <returns>The app secret id.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get()
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetSecretServiceModel
            {
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetSecretModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var secret = await this.secretsService.GetAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK, new { Id = secret });
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Creates an secret app.
        /// </summary>
        /// <returns>The app secret id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save()
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new CreateSecretServiceModel
            {
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new CreateSecretModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var secret = await this.secretsService.CreateAsync(serviceModel);

                if (secret is not null)
                {
                    return this.StatusCode((int)HttpStatusCode.OK, new { Id = secret.AppSecret });
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
