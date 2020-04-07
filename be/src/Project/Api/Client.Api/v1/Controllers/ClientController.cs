using Client.Api.v1.RequestModels;
using Client.Api.v1.Validators;
using Foundation.ApiExtensions.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        /// <summary>
        /// Creates a new client
        /// </summary>
        /// <param name="client">Client to save.</param>
        /// <returns>Account creation results.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Create([FromBody] ClientRequestModel client)
        {
            var validator = new ClientRequestModelValidator();
            var validationResult = await validator.ValidateAsync(client);

            if (validationResult.IsValid)
            {
                if (!client.Id.HasValue)
                {
                    return this.CreatedAtRoute("Create", new { });
                }

                return this.BadRequest();
            }

            return this.BadRequest(validationResult.Errors.ToString());
        }
    }
}
