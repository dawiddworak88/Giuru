using Api.v1.Areas.Schemas.RequestModels;
using Api.v1.Areas.Schemas.ResponseModels;
using Feature.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Helpers;
using Foundation.Extensions.Helpers;
using Foundation.Schema.Models;
using Foundation.Schema.Services.SchemaServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.v1.Areas.Schemas.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class SchemaController : BaseApiController
    {
        private readonly ISchemaService schemaService;

        private readonly ILogger logger;

        public SchemaController(ISchemaService schemaService, ILogger<SchemaController> logger)
        {
            this.schemaService = schemaService;
            this.logger = logger;
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
            try
            {
                var tenantClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.TenantIdClaim);

                var createSchemaModel = new CreateSchemaModel
                {
                    Name = schemaModel.Name,
                    EntityTypeId = schemaModel.EntityTypeId,
                    JsonSchema = JObject.Parse(schemaModel.JsonSchema),
                    UiSchema = JObject.Parse(schemaModel.UiSchema),
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    TenantId = GuidHelper.ParseNullable(tenantClaim?.Value),
                    Language = schemaModel.Language
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
            catch (Exception exception)
            {
                var error = ErrorHelper.GenerateErrorSignature(Assembly.GetExecutingAssembly().ToString());
                this.logger.LogError(exception, $"{error.ErrorId} - {error.ErrorSource}");
                return this.StatusCode((int)HttpStatusCode.BadRequest, new SchemaResponseModel { Error = error });
            }
        }
    }
}
