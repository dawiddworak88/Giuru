using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.Helpers;
using Identity.Api.Services.Organisations;
using Identity.Api.ServicesModels.Organisations;
using Identity.Api.v1.RequestModels;
using Identity.Api.v1.ResponseModels;
using Identity.Api.Validators.Organisations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize(AuthenticationSchemes = "IsToken" )]
    [ApiController]
    public class OrganisationsController : BaseApiController
    {
        private readonly IOrganisationService organisationService;

        public OrganisationsController(IOrganisationService organisationService)
        {
            this.organisationService = organisationService;
        }

        /// <summary>
        /// Gets the organisation by id.
        /// </summary>
        /// <param name="email">The organisation contact email.</param>
        /// <returns>The seller.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{email}")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(string email)
        {
            var serviceModel = new GetOrganisationModel
            {
                Email = email,
                Language = CultureInfo.CurrentCulture.Name
            };

            var validator = new GetOrganisationModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var organisation = await this.organisationService.GetAsync(serviceModel);

                if (organisation != null)
                {
                    var response = new OrganisationResponseModel
                    {
                        Id = organisation.Id,
                        Name = organisation.Name,
                        Description = organisation.Description,
                        Files = organisation.Files,
                        Images = organisation.Images,
                        Videos = organisation.Videos
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
                else
                {
                    return this.StatusCode((int)HttpStatusCode.NoContent);
                }
            }

            return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Creates an organisation.
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>The organisation id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(OrganisationResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save([FromBody] OrganisationRequestModel request)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            if (!request.Id.HasValue)
            {
                var serviceModel = new CreateOrganisationServiceModel
                {
                    Name = request.Name,
                    Email = request.Email,
                    CommunicationsLanguage = request.CommunicationLanguage,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new CreateOrganisationModelValidator();

                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var organisation = await this.organisationService.CreateAsync(serviceModel);

                    if (organisation != null)
                    {
                        var response = new OrganisationResponseModel
                        {
                            Id = organisation.Id,
                            Description = organisation.Description,
                            Name = organisation.Name,
                            Files = organisation.Files,
                            Images = organisation.Images,
                            Videos = organisation.Videos
                        };

                        return this.StatusCode((int)HttpStatusCode.Created, response);
                    }
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }

            return this.StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}
