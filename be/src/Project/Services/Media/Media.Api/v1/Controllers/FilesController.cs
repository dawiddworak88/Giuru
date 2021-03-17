using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Helpers;
using Media.Api.ServicesModels;
using Media.Api.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Media.Api.Services.Media;
using Media.Api.v1.Areas.Media.RequestModels;
using Foundation.ApiExtensions.Shared.Definitions;

namespace Media.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class FilesController : BaseApiController
    {
        private readonly IMediaService mediaService;

        public FilesController(IMediaService mediaService)
        {
            this.mediaService = mediaService;
        }

        /// <summary>
        /// Gets a file by media id. Provide width and height to resize the image files.
        /// </summary>
        /// <param name="mediaId">The media id to get.</param>
        /// <param name="o">The flag that indicates whether image file should be optimized.</param>
        /// <param name="w">The image width.</param>
        /// <param name="h">The image height.</param>
        /// <returns>The file.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [AllowAnonymous]
        [Route("{mediaId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof (FileContentResult))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get(Guid? mediaId, bool? o, int? w, int? h)
        {
            var mediaFile = await this.mediaService.GetFileAsync(mediaId, o, w, h);

            if (mediaFile != null)
            {
                this.Response.Headers[HeaderNames.CacheControl] = "public,max-age=" + CacheControlConstants.CacheControlMaxAgeSeconds;

                return this.File(mediaFile.File, mediaFile.ContentType, mediaFile.Filename);
            }

            return this.BadRequest();
        }

        /// <summary>
        /// Uploads a file.
        /// </summary>
        /// <param name="model">File contents.</param>
        /// <returns>Created if the file has been uploaded successfully.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Guid))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [RequestSizeLimit(ApiConstants.Request.RequestSizeLimit)]
        public async Task<IActionResult> Create([FromForm] UploadMediaRequestModel model)
        {
            if (model.File == null)
            {
                return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
            }

            var organisationClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim);

            var serviceModel = new CreateMediaItemServiceModel
            {
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(organisationClaim?.Value),
                Language = CultureInfo.CurrentCulture.Name,
                File = model.File
            };

            var validator = new CreateMediaItemModelValidator();

            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var mediaItemId = await this.mediaService.CreateFileAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.Created, new { Id = mediaItemId });
            }

            return this.StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}
