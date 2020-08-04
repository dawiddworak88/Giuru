using Catalog.Api.v1.Areas.Clients.RequestModels;
using Catalog.Api.v1.Areas.Clients.ResponseModels;
using Catalog.Api.v1.Areas.Clients.Definitions;
using Catalog.Api.v1.Areas.Clients.Models;
using Catalog.Api.v1.Areas.Clients.Services;
using Foundation.ApiExtensions.Controllers;
using Foundation.ApiExtensions.Helpers;
using Foundation.Extensions.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Foundation.Account.Definitions;

namespace Catalog.Api.v1.Areas.Clients.Controllers
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
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> Create([FromBody] ClientRequestModel clientModel)
        {
            try
            {
                var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.SellerIdClaim);

                var createClientModel = new CreateClientModel
                {
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    SellerId = GuidHelper.ParseNullable(sellerClaim?.Value),
                    Name = clientModel.Name,
                    Email = clientModel.Email,
                    Language = clientModel.Language,
                    ClientPreferredLanguage = clientModel.CommunicationLanguage
                };

                var createClientResult = await this.clientService.CreateAsync(createClientModel);

                if (createClientResult.IsValid)
                {
                    return this.StatusCode((int)HttpStatusCode.Created, new ClientResponseModel(createClientResult.Client));
                }
                else
                {
                    if (createClientResult.Errors.Contains(ErrorConstants.DuplicateUser))
                    {
                        return this.StatusCode((int)HttpStatusCode.Conflict);
                    }

                    return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
                }
            }
            catch (Exception exception)
            {
                var error = ErrorHelper.GenerateErrorSignature(Assembly.GetExecutingAssembly().ToString());
                this.logger.LogError(exception, $"{error.ErrorId} - {error.ErrorSource}");
                return this.StatusCode((int)HttpStatusCode.BadRequest, new ClientResponseModel { Error = error });
            }
        }
    }
}
