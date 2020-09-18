using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Helpers;
using Media.Api.v1.Area.Media.ApiRequestModels;
using Media.Api.v1.Area.Media.Models;
using Media.Api.v1.Area.Media.Services;
using Media.Api.v1.Area.Media.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
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
        [Route("{mediaId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Get(Guid? mediaId)
        {
            var mediaFile = await this.mediaService.GetMediaItemAsync(mediaId);

            if (mediaFile != null)
            {
                this.Response.Headers[HeaderNames.CacheControl] = "public,max-age=" + CacheControlConstants.CacheControlMaxAgeSeconds;

                return this.File(mediaFile.File, mediaFile.ContentType, mediaFile.Filename);
            }

            return this.BadRequest();
        }

        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromForm] UploadMediaRequestModel model)
        {
            if (model.File == null)
            {
                return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
            }

            var organisationClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim);

            var serviceModel = new CreateMediaItemModel
            {
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                SellerId = GuidHelper.ParseNullable(organisationClaim?.Value),
                Language = model.Language,
                File = model.File
            };

            var validator = new CreateMediaItemModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var mediaItemId = await this.mediaService.CreateMediaItemAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.Created, new { Id = mediaItemId });
            }

            return this.StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}
