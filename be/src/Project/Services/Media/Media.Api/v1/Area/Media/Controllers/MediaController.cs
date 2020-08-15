using Foundation.ApiExtensions.Controllers;
using Media.Api.v1.Area.Media.Services;
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
        private readonly IMediaService mediaService;

        public MediaController(IMediaService mediaService)
        {
            this.mediaService = mediaService;
        }

        [HttpGet, MapToApiVersion("1.0")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Get(Guid? mediaId)
        {
            var mediaFile = await this.mediaService.GetMediaItemAsync(mediaId);

            return this.File(mediaFile.File, mediaFile.ContentType, mediaFile.Filename);
        }
    }
}
