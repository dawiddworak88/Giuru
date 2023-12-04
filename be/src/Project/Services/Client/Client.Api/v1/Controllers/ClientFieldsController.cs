using Client.Api.Services.Fields;
using Client.Api.ServicesModels.Addresses;
using Client.Api.ServicesModels.Fields;
using Client.Api.v1.RequestModels;
using Client.Api.Validators.Addresses;
using Client.Api.Validators.Feilds;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Client.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Authorize]
    [ApiController]
    public class ClientFieldsController : BaseApiController
    {
        private readonly IClientFieldsService _clientFieldsService;

        public ClientFieldsController(
            IClientFieldsService clientFieldsService)
        {
            _clientFieldsService = clientFieldsService;
        }

        /// <summary>
        /// Creates or updates client field (if client field id is set).
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>The client field id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save([FromBody] ClientFieldRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            if (request.Id.HasValue)
            {
                return default;

                //throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new CreateClientFieldServiceModel
                {
                    Name = request.Name,
                    Type = request.Type,
                    IsRequired = request.IsRequired,
                    Options = request.Options.Select(x => new ClientFieldOptionServiceModel
                    {
                        Name = x.Name,
                        Value = x.Value
                    }),
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new CreateClientFieldModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var clientFieldId = await _clientFieldsService.CreateAsync(serviceModel);

                    return StatusCode((int)HttpStatusCode.OK, new { Id = clientFieldId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }
    }
}
