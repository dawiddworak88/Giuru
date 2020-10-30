using Foundation.ApiExtensions.Controllers;
using Identity.Api.v1.Areas.Accounts.Models;
using Identity.Api.v1.Areas.Accounts.Services.Organisations;
using Identity.Api.v1.Areas.Accounts.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Identity.Api.v1.Areas.Accounts.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class SellersController : BaseApiController
    {
        private readonly ISellerService organisationService;

        public SellersController(ISellerService organisationService)
        {
            this.organisationService = organisationService;
        }

        /// <summary>
        /// Gets the seller by id.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="id">The id.</param>
        /// <returns>The seller.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Get(string language, Guid? id)
        {
            var serviceModel = new GetSellerModel
            {
                Id = id,
                Language = language
            };

            var validator = new GetSellerModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var seller = await this.organisationService.GetAsync(serviceModel);

                return seller != null ? this.StatusCode((int)HttpStatusCode.OK, seller) : (IActionResult)this.StatusCode((int)HttpStatusCode.NotFound);
            }

            return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
