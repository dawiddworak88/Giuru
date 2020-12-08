using Catalog.Api.v1.Areas.Taxonomies.RequestModels;
using Catalog.Api.v1.Areas.Taxonomies.ResponseModels;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Helpers;
using Catalog.Api.v1.Areas.Taxonomies.Models;
using Catalog.Api.v1.Areas.Taxonomies.Services.TaxonomyServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Catalog.Api.v1.Areas.Taxonomies.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class TaxonomyController : BaseApiController
    {
        private readonly ITaxonomyService taxonomyService;

        public TaxonomyController(ITaxonomyService taxonomyService)
        {
            this.taxonomyService = taxonomyService;
        }

        /// <summary>
        /// Creates a new taxonomy
        /// </summary>
        /// <param name="taxonomyModel">Taxonomy to save.</param>
        /// <returns>Taxonomy creation results.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Create([FromBody] TaxonomyRequestModel taxonomyModel)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim);

            var createTaxonomyModel = new CreateTaxonomyModel
            {
                Name = taxonomyModel.Name,
                ParentId = taxonomyModel.ParentId,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Language = taxonomyModel.Language
            };

            var createTaxonomyResult = await this.taxonomyService.CreateAsync(createTaxonomyModel);

            if (createTaxonomyResult.IsValid)
            {
                return this.StatusCode((int)HttpStatusCode.Created, new TaxonomyResponseModel { Id = createTaxonomyResult.Taxonomy.Id });
            }
            else
            {
                return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Gets the taxonomy. Leave the rootId empty to get the whole tree.
        /// </summary>
        /// <param name="name">The name of the taxonomy to get.</param>
        /// <param name="rootId">The root id of the taxonomy.</param>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Get(string name, Guid? rootId, string language)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim);

            var getTaxonomyModel = new GetTaxonomyModel
            {
                Name = name,
                RootId = rootId,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Language = language
            };

            var getTaxonomyResult = await this.taxonomyService.GetByName(getTaxonomyModel);

            if (getTaxonomyResult.IsValid)
            {
                return this.StatusCode((int)HttpStatusCode.OK, new TaxonomyResponseModel { Id = getTaxonomyResult.Taxonomy?.Id });
            }
            else
            {
                return this.StatusCode((int)HttpStatusCode.NotFound);
            }
        }
    }
}
