using Foundation.ApiExtensions.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Media.Api.v1.Area.Media.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class MediaController : BaseApiController
    {
        [HttpGet, MapToApiVersion("1.0")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Get(Guid mediaId)
        {
            return this.Ok(); // this.File(, );
        }
    }
}
