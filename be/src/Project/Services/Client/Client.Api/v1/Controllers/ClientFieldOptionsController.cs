using Client.Api.Services.FieldOptions;
using Client.Api.ServicesModels.FieldOptions;
using Client.Api.v1.RequestModels;
using Client.Api.Validators.FieldOptions;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.Helpers;
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
    public class ClientFieldOptionsController : BaseApiController
    {
        private readonly IClientFieldOptionsService _clientFieldOptionsService;

        public ClientFieldOptionsController(
            IClientFieldOptionsService clientFieldOptionsService) 
        { 
            _clientFieldOptionsService = clientFieldOptionsService;
        }

        /// <summary>
        /// Creates or updates client field option (if client field option id is set).
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>The client field option id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save([FromBody] ClientFieldOptionRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            if (request.Id.HasValue)
            {
                var serviceModel = new UpdateFieldOptionServiceModel
                {
                    Id = request.Id,
                    Name = request.Name,
                    Value = request.Value,
                    FieldDefinitionId = request.FieldDefinitionId,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new UpdateFieldOptionModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var clientFieldOptionId = await _clientFieldOptionsService.UpdateAsync(serviceModel);

                    return StatusCode((int)HttpStatusCode.OK, new { Id = clientFieldOptionId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new CreateFieldOptionServiceModel
                {
                    Name = request.Name,
                    Value = request.Value,
                    FieldDefinitionId = request.FieldDefinitionId,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new CreateFieldOptionModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var clientFieldOptionId = await _clientFieldOptionsService.CreateAsync(serviceModel);

                    return StatusCode((int)HttpStatusCode.OK, new { Id = clientFieldOptionId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }
    }
}
