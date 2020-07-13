using Api.v1.Areas.Clients.ResponseModels;
using Feature.Account.Definitions;
using Feature.Client.Models;
using Feature.Client.Services;
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

namespace Api.v1.Areas.Clients.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class ClientsController : BaseApiController
    {
        private readonly IClientService clientService;

        private readonly ILogger logger;

        public ClientsController(IClientService clientService, ILogger<ClientController> logger)
        {
            this.clientService = clientService;
            this.logger = logger;
        }
        /// <summary>
        /// Returns clients by search term. Returns all clients (paginated) if search term is empty.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The number of items per page.</param>
        /// <returns></returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Get(string language, string searchTerm, int pageIndex, int itemsPerPage)
        {
            try
            {
                var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.SellerIdClaim);

                var getClientsModel = new GetClientsModel
                {
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    SearchTerm = searchTerm,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    SellerId = GuidHelper.ParseNullable(sellerClaim?.Value),
                    Language = language
                };

                var getClientsResult = await this.clientService.GetAsync(getClientsModel);

                if (getClientsResult.IsValid)
                {
                    return this.StatusCode((int)HttpStatusCode.OK, new ClientsResponseModel(getClientsResult.Clients));
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
                return this.StatusCode((int)HttpStatusCode.BadRequest, new ClientsResponseModel { Error = error });
            }
        }
    }
}
