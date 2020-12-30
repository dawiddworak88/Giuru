using Catalog.Api.v1.Areas.Schemas.RequestModels;
using Catalog.Api.v1.Areas.Schemas.ResponseModels;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Helpers;
using Catalog.Api.v1.Areas.Schemas.Models;
using Catalog.Api.v1.Areas.Schemas.Services.SchemaServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Globalization;

namespace Catalog.Api.v1.Areas.Schemas.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class SchemaController : BaseApiController
    {
        private readonly ISchemaService schemaService;

        public SchemaController(ISchemaService schemaService)
        {
            this.schemaService = schemaService;
        }

        /// <summary>
        /// Creates a new schema
        /// </summary>
        /// <param name="schemaModel">Schema to save.</param>
        /// <returns>Schema creation results.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Create([FromBody] SchemaRequestModel schemaModel)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim);

            var createSchemaModel = new CreateSchemaModel
            {
                Name = schemaModel.Name,
                EntityTypeId = schemaModel.EntityTypeId,
                JsonSchema = !string.IsNullOrWhiteSpace(schemaModel.JsonSchema) ? JObject.Parse(schemaModel.JsonSchema) : null,
                UiSchema = !string.IsNullOrWhiteSpace(schemaModel.UiSchema) ? JObject.Parse(schemaModel.UiSchema) : null,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Language = CultureInfo.CurrentCulture.Name
            };

            var createSchemaResult = await this.schemaService.CreateAsync(createSchemaModel);

            if (createSchemaResult.IsValid)
            {
                return this.StatusCode((int)HttpStatusCode.Created, new SchemaResponseModel { Id = createSchemaResult.Schema.Id });
            }
            else
            {
                return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Gets a schema by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The schema.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(Guid? id)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim);

            var getSchemaModel = new GetSchemaModel
            {
                Id = id,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Language = CultureInfo.CurrentCulture.Name
            };

            var getSchemaResult = await this.schemaService.GetByIdAsync(getSchemaModel);

            if (getSchemaResult.IsValid)
            {
                return this.StatusCode((int)HttpStatusCode.OK, new SchemaResponseModel(getSchemaResult.Schema));
            }
            else
            {
                if (getSchemaResult.Errors.Contains(ErrorConstants.NotFound))
                {
                    return this.StatusCode((int)HttpStatusCode.NotFound);
                }

                return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Gets the schema by entity type id.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="id">The entity type id.</param>
        /// <returns>The schema.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("EntityType")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByEntityTypeId(Guid? id)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim);

            var getSchemaModel = new GetSchemaByEntityTypeModel
            {
                EntityTypeId = id,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Language = CultureInfo.CurrentCulture.Name
            };

            var getSchemaResult = await this.schemaService.GetByEntityTypeIdAsync(getSchemaModel);

            if (getSchemaResult.IsValid)
            {
                return this.StatusCode((int)HttpStatusCode.OK, new SchemaResponseModel(getSchemaResult.Schema));
            }
            else
            {
                if (getSchemaResult.Errors.Contains(ErrorConstants.NotFound))
                {
                    return this.StatusCode((int)HttpStatusCode.NotFound);
                }

                return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
            }
        }
    }
}
