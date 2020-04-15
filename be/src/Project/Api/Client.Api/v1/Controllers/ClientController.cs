using Client.Api.v1.RequestModels;
using Feature.Account.Definitions;
using Feature.Client.Models;
using Feature.Client.Services;
using Foundation.ApiExtensions.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Client.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class ClientController : BaseApiController
    {
        private readonly IClientService clientService;

        private readonly ILogger logger;

        public ClientController(IClientService clientService, ILogger<ClientController> logger)
        {
            this.clientService = clientService;
            this.logger = logger;
        }

        /// <summary>
        /// Creates a new client
        /// </summary>
        /// <param name="clientModel">Client to save.</param>
        /// <returns>Client creation results.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] ClientRequestModel clientModel)
        {
            try
            {
                var tenantClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.TenantIdClaim);

                var createClientModel = new CreateClientModel
                {
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    TenantId = !string.IsNullOrWhiteSpace(tenantClaim?.Value) ? Guid.Parse(tenantClaim.Value) : Guid.Empty,
                    Name = clientModel.Name,
                    Email = clientModel.Email,
                    Language = clientModel.Language
                };

                var createClientResult = await this.clientService.CreateAsync(createClientModel);

                if (createClientResult.ValidationResult.IsValid)
                {
                    return this.Ok();
                }

                return this.BadRequest(createClientResult.ValidationResult.Errors.ToString());
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, nameof(Create));
            }

            return this.BadRequest();
        }
    }
}
