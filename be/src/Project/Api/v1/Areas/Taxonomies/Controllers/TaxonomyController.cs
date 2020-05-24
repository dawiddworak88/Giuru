using Api.v1.Areas.Taxonomies.RequestModels;
using Api.v1.Areas.Taxonomies.ResponseModels;
using Feature.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Helpers;
using Foundation.Extensions.Helpers;
using Foundation.Taxonomy.Models;
using Foundation.Taxonomy.Services.TaxonomyServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.v1.Areas.Taxonomies.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class TaxonomyController : BaseApiController
    {
        private readonly ITaxonomyService taxonomyService;

        private readonly ILogger logger;

        public TaxonomyController(ITaxonomyService taxonomyService, ILogger<TaxonomyController> logger)
        {
            this.taxonomyService = taxonomyService;
            this.logger = logger;
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
            try
            {
                var tenantClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.TenantIdClaim);

                var createTaxonomyModel = new CreateTaxonomyModel
                {
                    Name = taxonomyModel.Name,
                    ParentId = taxonomyModel.ParentId,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    TenantId = GuidHelper.ParseNullable(tenantClaim?.Value),
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
            catch (Exception exception)
            {
                var error = ErrorHelper.GenerateErrorSignature(Assembly.GetExecutingAssembly().ToString());
                this.logger.LogError(exception, $"{error.ErrorId} - {error.ErrorSource}");
                return this.StatusCode((int)HttpStatusCode.BadRequest, new TaxonomyResponseModel { Error = error });
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
            try
            {
                var tenantClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.TenantIdClaim);

                var getTaxonomyModel = new GetTaxonomyModel
                {
                    Name = name,
                    RootId = rootId,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    TenantId = GuidHelper.ParseNullable(tenantClaim?.Value),
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
            catch (Exception exception)
            {
                var error = ErrorHelper.GenerateErrorSignature(Assembly.GetExecutingAssembly().ToString());
                this.logger.LogError(exception, $"{error.ErrorId} - {error.ErrorSource}");
                return this.StatusCode((int)HttpStatusCode.BadRequest, new TaxonomyResponseModel { Error = error });
            }
        }
    }
}
