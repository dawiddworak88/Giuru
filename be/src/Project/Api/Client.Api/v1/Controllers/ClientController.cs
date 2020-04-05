using Client.Api.v1.RequestModels;
using Foundation.Extensions.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [AllowAnonymous]
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
        public IActionResult Create([FromBody] ClientRequestModel client)
        {
            return this.Ok();
        }
    }
}
