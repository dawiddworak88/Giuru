using Foundation.ApiExtensions.Controllers;
using Identity.Api.Services.Organisations;
using Identity.Api.ServicesModels.Organisations;
using Identity.Api.v1.ResponseModels;
using Identity.Api.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace Identity.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class SellersController : BaseApiController
    {
        private readonly IOrganisationService organisationService;

        public SellersController(IOrganisationService organisationService)
        {
            this.organisationService = organisationService;
        }

        /// <summary>
        /// Gets the seller by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The seller.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(Guid? id)
        {
            var serviceModel = new GetSellerModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name
            };

            var validator = new GetSellerModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var seller = await this.organisationService.GetAsync(serviceModel);

                if (seller != null)
                {
                    var response = new OrganisationResponseModel
                    { 
                        Id = seller.Id,
                        Name = seller.Name,
                        Description = seller.Description,
                        Files = seller.Files,
                        Images = seller.Images,
                        Videos = seller.Videos
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
    }
}
